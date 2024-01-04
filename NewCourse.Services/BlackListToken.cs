using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory; 


//namespace NewCourse.Services
//{
//    public class BlacklistService 
//    {
//        private readonly ICacheProvider _cacheProvider;

//        public BlacklistService(ICacheProvider cacheProvider)
//        {
//            _cacheProvider = cacheProvider;
//        }

//        public bool AddTokenToBlacklist(string token)
//        {
//            return _cacheProvider.SetItem(token, true, TimeSpan.FromHours(1)); // Adjust expiry time as needed
//        }

//        public bool IsTokenBlacklisted(string token)
//        {
//            return _cacheProvider.GetItem<bool>(token);
//        }
//    }
//}
