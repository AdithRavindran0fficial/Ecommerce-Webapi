using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Data.UserDtOs
{
    public class OutUsers
    {

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string? Phone { get; set; }
    }
}
