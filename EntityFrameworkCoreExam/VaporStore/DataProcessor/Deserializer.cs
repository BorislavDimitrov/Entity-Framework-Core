namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var gameDtos = JsonConvert.DeserializeObject<ICollection<GameInputModel>>(jsonString);

            foreach (var game in gameDtos)
            {
                if (!IsValid(game) || game.Tags.Count == 0)
                {
					sb.AppendLine("Invalid Data");
					continue;
                }

				var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer);
                if (developer == null)
                {
					developer = new Developer { Name = game.Developer };
                }

				var genre = context.Genres.FirstOrDefault(x => x.Name == game.Genre);
                if (genre == null)
                {
					genre = new Genre { Name = game.Genre };
                }

				var newGame = new Game
				{
					Name = game.Name,
					Price = game.Price,
					ReleaseDate = DateTime.ParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
					Developer = developer,
					Genre = genre
				};

                foreach (var gameTag in game.Tags)
                {
					var tag = context.Tags.FirstOrDefault(x => x.Name == gameTag);
                    if (tag == null)
                    {
						tag = new Tag { Name = gameTag };
                    }

					newGame.GameTags.Add(new GameTag { Tag = tag });
                }

				sb.AppendLine($"Added {newGame.Name} ({newGame.Genre.Name}) with {newGame.GameTags.Count} tags");
				context.Games.Add(newGame);
				context.SaveChanges();
			}
			
			return sb.ToString();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var userDtos = JsonConvert.DeserializeObject<ICollection<UserInputModel>>(jsonString);

            foreach (var user in userDtos)
            {
                if (!IsValid(user) || user.Cards.Count == 0)
                {
					sb.AppendLine("Invalid Data");
					continue;
                }

				var newUser = new User
				{
					FullName = user.FullName,
					Username = user.Username,
					Email = user.Email,
					Age = user.Age
				};

				bool isValidd = true;

                foreach (var card in user.Cards)
                {
                    if (!IsValid(card))
                    {
						sb.AppendLine("Invalid Data");
						isValidd = false;
						break;
                    }

					CardType type = (CardType)Enum.Parse(typeof(CardType), card.Type);

					var newCard = new Card
					{
						Number = card.Number,
						Cvc = card.CVC,
						Type = type
					};

					newUser.Cards.Add(newCard);
                }

                if (isValidd == false)
                {
					continue;
                }

				context.Users.Add(newUser);
				sb.AppendLine($"Imported {newUser.Username} with {newUser.Cards.Count} cards");
            }
			context.SaveChanges();

			return sb.ToString();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var serializer = new XmlSerializer(typeof(List<PurchaseInputModel>), new XmlRootAttribute("Purchases"));

			var purchaseDtos = serializer.Deserialize(new StringReader(xmlString)) as List<PurchaseInputModel>;

            foreach (var purchase in purchaseDtos)
            {
                if (!IsValid(purchase))
                {
					sb.AppendLine("Invalid Data");
					continue;
                }

				var game = context.Games.FirstOrDefault(x => x.Name == purchase.Title);
                if (game == null)
                {
					sb.AppendLine("Invalid Data");
					continue;
				}

                if (Enum.IsDefined(typeof(PurchaseType), purchase.Type) == false)
                {
					sb.AppendLine("Invalid Data");
					continue;
				}

				var card = context.Cards.FirstOrDefault(x => x.Number == purchase.Card);
                if (card == null)
                {
					sb.AppendLine("Invalid Data");
					continue;
				}

				PurchaseType type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchase.Type);

				var newPurchase = new Purchase
				{
					Type = type,
					ProductKey = purchase.Key,
					Date = DateTime.ParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
					Card = card,
					Game = game
				};

				context.Purchases.Add(newPurchase);

				sb.AppendLine($"Imported {newPurchase.Game.Name} for {newPurchase.Card.User.Username}");
            }

			context.SaveChanges();

			return sb.ToString();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}