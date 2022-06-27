using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class PurchaseExportModel
    {
    
        public string Card { get; set; }

        
        public string Cvc { get; set; }

        
        public string Date { get; set; }

        public GameExportModel Game { get; set; }
    }
}