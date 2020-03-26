using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Jwt.Model
{
    public class UserEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

    }
}
