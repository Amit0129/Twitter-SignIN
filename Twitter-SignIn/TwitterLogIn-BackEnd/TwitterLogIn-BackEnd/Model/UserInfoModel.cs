using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterLogIn_BackEnd.Model
{
    public class UserInfoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserTableId { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
    }
}
