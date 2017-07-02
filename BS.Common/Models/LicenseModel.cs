using System;

namespace BS.Api.Models
{
    public class LicenseModel
    {
        public Guid Id { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsDemo { get; set; }

        public override string ToString()
        {
            return string.Format(
@"Идентификатор: {0},
Валиден до: {1},
Демо: {2}", Id, ValidTo, IsDemo ? "Да" : "Не");
        }
    }
}