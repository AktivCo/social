using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AdUsers
{
    public class AdUserProvider
    {
        private string DOMAIN;
        const int UF_ACCOUNTDISABLE = 0x0002;
        private const string AD_USERS = "ADusers";

        public AdUserProvider(string domain)
        {
            this.DOMAIN = domain;
        }

        public AdUser GetStructure(AdUser root)
        {
            List<AdUser> users = GetAllUsers().Where(usr => !usr.Disabled).ToList();
            var rootUsers = users.Where(u => u.Manager == root.CN).ToList();
            if (rootUsers.Any(usr => !usr.Disabled))
            {

                rootUsers.ForEach(user => GetStructure(user));
                root.Users = rootUsers.OrderByDescending(usr => usr.Department).ToList();
            }
            return root;
        }
       

        public AdUser GetUserByLogin(string login)
        {
            return GetAllUsers().FirstOrDefault(u => u.Login == login);
        }

        private AdUser GetUserFromEntry(DirectoryEntry di)
        {
            int phone = 0;
            if (di.Properties["telephoneNumber"].Value != null)
            {
                int.TryParse(di.Properties["telephoneNumber"].Value.ToString(), out phone);
            }
            DateTime created = DateTime.MinValue;
            if (di.Properties["whenCreated"].Value != null)
            {
                // DateTime.ParseExact(di.Properties["whenCreated"].Value.ToString())
                created = (DateTime)di.Properties["whenCreated"].Value;
            }

            int flags = (int)di.Properties["userAccountControl"][0];

            string manager = di.Properties["manager"].Value != null
                                 ? GetCnFromString(di.Properties["manager"].Value.ToString())
                                 : string.Empty;

            DateTime? lastLogon = null;



            //if (di.Properties.Contains("lastLogon"))
            //{

            //    var largeInt = (LargeInteger)di.Properties["lastLogon"][0];
            //    Int64 liTicks = largeInt.HighPart * 0x100000000 + largeInt.LowPart;
            //    if (DateTime.MaxValue.Ticks >= liTicks && DateTime.MinValue.Ticks <= liTicks)
            //    {
            //        lastLogon = DateTime.FromFileTime(liTicks);
            //    }
            //}
            var user = new AdUser()
            {

                Login = di.Properties["sAMAccountName"].Value != null ? di.Properties["sAMAccountName"].Value.ToString() : string.Empty,
                CN = di.Properties["cn"].Value != null ? di.Properties["cn"].Value.ToString() : string.Empty,
                FullName = di.Properties["displayName"].Value != null ? di.Properties["displayName"].Value.ToString() : string.Empty,
                Department = di.Properties["department"].Value != null ? di.Properties["department"].Value.ToString() : string.Empty,
                Mobile = di.Properties["mobile"].Value != null ? di.Properties["mobile"].Value.ToString() : string.Empty,
                Office = di.Properties["physicalDeliveryOfficeName"].Value != null ? di.Properties["physicalDeliveryOfficeName"].Value.ToString() : string.Empty,
                Title = di.Properties["title"].Value != null ? di.Properties["title"].Value.ToString() : string.Empty,
                Mail = di.Properties["mail"].Value != null ? di.Properties["mail"].Value.ToString() : string.Empty,
                WhenCreated = di.Properties["whenCreated"].Value != null ? (DateTime)di.Properties["whenCreated"].Value : DateTime.Now,
                OfficePhone = phone == 0 ? (int?)null : phone,
                HasPicture = di.Properties["thumbnailPhoto"].Value != null,
                CreatedWhen = created,
                Disabled = Convert.ToBoolean(flags & UF_ACCOUNTDISABLE),
                Userpic = di.Properties["thumbnailPhoto"].Value != null ? (byte[])di.Properties["thumbnailPhoto"].Value : null,
                Category = di.Properties["objectCategory"].Value != null ? di.Properties["objectCategory"].Value.ToString() : string.Empty,
                sAMAccountType = di.Properties["sAMAccountType"].Value != null ? (int)di.Properties["sAMAccountType"].Value : 0,
                LastLogon = lastLogon,
                Manager = manager,
                Sid = di.Properties["objectSid"].Value != null ? new SecurityIdentifier((byte[])di.Properties["objectSid"].Value, 0).ToString() : string.Empty
            };
            di.Close();
            di.Dispose();

            return user;
        }

        private string GetCnFromString(string manager)
        {
            // todo:regexp?
            return manager.Remove(manager.IndexOf(",OU=", StringComparison.Ordinal)).Replace("CN=", string.Empty);
        }

        public List<AdUser> GetAllUsers()
        {
            List<AdUser> users = HttpRuntime.Cache[AD_USERS] as List<AdUser>;
            if (users == null)
            {
                FillCache();
                users = HttpRuntime.Cache[AD_USERS] as List<AdUser>;
                if (users == null)
                {
                    throw new Exception("no data");
                }
            }
            return users;
        }

        private List<AdUser> GetUsers()
        {
            string filter = string.Format("(&(ObjectClass={0}))", "user");
            // filter = "(&(instanceType=4))";

            //   string[] properties = new string[] { "fullname" };

            DirectoryEntry adRoot = new DirectoryEntry("LDAP://" + DOMAIN, null, null, AuthenticationTypes.Secure);
            DirectorySearcher searcher = new DirectorySearcher(adRoot);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.ReferralChasing = ReferralChasingOption.All;
            //  searcher.PropertiesToLoad.AddRange(properties);
            searcher.Filter = filter;

            SearchResultCollection result = searcher.FindAll();

            return (from SearchResult searchResult in result select GetUserFromEntry(searchResult.GetDirectoryEntry())).ToList();

        }


        public bool Validate(string login, string pass)
        {
            
            using (var context = new System.DirectoryServices.AccountManagement.PrincipalContext(ContextType.Domain, DOMAIN, null, null))
            {

                //Username and password for authentication.
                return context.ValidateCredentials(login, pass);
            }
        }
        private DirectoryEntry GetEntry(string login)
        {

            string filter = string.Format("(&(ObjectClass={0})(sAMAccountName={1}))", "person", login);

            string[] properties = new string[] { "fullname" };

            DirectoryEntry adRoot = new DirectoryEntry("LDAP://" + DOMAIN, null, null, AuthenticationTypes.Secure);
            DirectorySearcher searcher = new DirectorySearcher(adRoot);
            searcher.SearchScope = SearchScope.Subtree;
            searcher.ReferralChasing = ReferralChasingOption.All;
            searcher.PropertiesToLoad.AddRange(properties);
            searcher.Filter = filter;

            SearchResult result = searcher.FindOne();
            DirectoryEntry directoryEntry = result.GetDirectoryEntry();
            return directoryEntry;
        }

        public void FillCache()
        {
            HttpRuntime.Cache.Insert(AD_USERS, GetUsers(), null, DateTime.UtcNow.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, new CacheItemRemovedCallback(OnCacheRemove));
        }

        private void OnCacheRemove(string key, object value, CacheItemRemovedReason reason)
        {
            FillCache();
        }


    }
}
