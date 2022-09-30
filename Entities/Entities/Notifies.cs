using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    public class Notifies
    {
        [NotMapped]
        public string NameProperty { get; set; }

        [NotMapped]
        public string Message { get; set; }

        [NotMapped]
        public List<Notifies> Notifications { get; set; } = new List<Notifies>();

        public bool ValidateStringProperty(string value, string nameProperty)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(nameProperty))
            {
                Notifications.Add(new Notifies
                {
                    NameProperty = nameProperty,
                    Message = "Campo Obrigatório"
                });
                return false;
            }
            return true;
        }

        public bool ValidateIntProperty(int value, string nameProperty)
        {
            if (value < 1 || string.IsNullOrWhiteSpace(nameProperty))
            {
                Notifications.Add(new Notifies
                {
                    NameProperty = nameProperty,
                    Message = "Campo Obrigatório"
                });
                return false;
            }
            return true;
        }
    }
}
