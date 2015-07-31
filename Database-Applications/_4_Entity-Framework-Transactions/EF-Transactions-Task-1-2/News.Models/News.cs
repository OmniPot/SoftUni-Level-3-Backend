namespace News.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class News
    {
        public int Id { get; set; }

        public string NewsContent { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
