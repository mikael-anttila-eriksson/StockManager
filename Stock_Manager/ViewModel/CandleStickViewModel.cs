using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Stock_Manager.ViewModel
{
    // Use for Serializing Data - required to serve in JSON format
    [DataContract]
    public class CandleStickViewModel
    {
        // Explicitly setting the name to be used while serializing to JSON
        [DataMember(Name = "X")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy'-'MM'-'dd}")]
        [DataType(DataType.Date)]
        public string Day { get; set; }
        [DataMember(Name = "Y")]
        public double[] dayPrice { get; set; } //= new double[4];
        // Candle6
        [DataMember(Name = "Open")]
        public double Open { get; set; }
        [DataMember(Name = "Close")]
        public double Close { get; set; }
        [DataMember(Name = "High")]
        public double High { get; set; }
        [DataMember(Name = "Low")]
        public double Low { get; set; }
        [DataMember(Name = "Volume")]
        public int? Volume { get; set; }        
    }
}
