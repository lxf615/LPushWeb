using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using RestSharp;
using log4net;
using Generic;

namespace Demo.Test
{
    public class TestDemo
    {
        public void test(object j)
        {
            lock (this)
            {
                int i = (int)j;
                if (i > 10)
                {
                    i--;
                    test(i);
                }

            }
        }

        public int this[int index]
        {
            get
            {
                return 1;
            }
            set
            {
                ;
            }
        }

        public int Foo(int i)
        {
            if (i <= 0)
            {
                return 0;
            }
            else if (i <= 2)
            {
                return 1;
            }
            else
            {
                return Foo(i - 2) + Foo(i - 1);
            }
        }




        public static void Test()
        {
            int[] array = new int[] { 2, 1, 9, 8, 5, 4 };
            bool foo = false;
            do
            {
                foo = false;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                        foo = true;
                    }
                }
            } while (foo);
        }
    }


    class Program
    {
        static string url = "http://localhost:52150/";
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            //string url = "https://q.nuomi.com/qstore/oapi/sendcard";

            string url = "http://localhost:50291/api/QStore/BindCompany";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("companyId", "32");
            dic.Add("companyName", "test");
            //dic.Add("totalMoney", "1");
            //dic.Add("batchId", "7");
            //dic.Add("cardDetail", SimpleJson.SerializeObject(new List<dynamic>
            //    {
            //        new {startId="100",endId="105",price=400,tailData=new List<int>() {1,3,4,4 } },
            //    }));
            //dic.Add("mac", "d0b9c806978197e3c53861ef3432d21a");
            //dic.Add("appId", "301");

            HttpWebResponse response = HttpWebUtils.Post(url, dic);

            using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            Console.ReadLine();
        }



        public static void Test()
        {
            Console.WriteLine("按回车发送");
            Console.ReadLine();

            //RestClient client = new RestClient("http://lpush.webapi2/");
            RestClient client = new RestClient("http://localhost:55073/");

            RestRequest request = new RestRequest("api/account/GetUser", Method.POST);

            DateTime now = DateTime.Now;
            var obj = new
            {
                Id = 123,
            };
            string json = SimpleJson.SerializeObject(obj);

            request.AddHeader("Sign", ("Test|" + now.ToString()).ToBase64());
            request.AddHeader("Token", string.Format("{0}|{1}",now.ToString(),json).ToMD5());

            request.AddJsonBody(EncryptUtils.AESEncrypt(json, "d3d3Lnl1ZnUuY253d3cuZnVrYS5jYw==", "d3d3LmZ1a2EuY2M="));

            IRestResponse response = client.Execute(request);
            string res = response.Content;
            res = EncryptUtils.AESDecrypt(res.Trim(), "d3d3Lnl1ZnUuY253d3cuZnVrYS5jYw==", "d3d3LmZ1a2EuY2M=");
            dynamic result = SimpleJson.DeserializeObject<dynamic>(res);
            Console.WriteLine(res);
            Console.ReadLine();

        }

        public static string GetClientToken()
        {

            RestClient client = new RestClient(url);

            RestRequest request = new RestRequest("Token");
            request.AddHeader("Authorization", "Basic " +
                Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("admin:123456")));
            request.AddParameter("grant_type", "client_credentials");
            //request.AddObject(new
            //{
            //    grant_type = "client_credentials",
            //    //clientId = "admin",
            //    //clientSecret = "123456"
            //});

            IRestResponse response = client.Post(request);
            dynamic result = SimpleJson.DeserializeObject<dynamic>(response.Content);
            return result.access_token;


        }

        public static string CancelNotify()
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest("CancelNotify");
            request.AddHeader("Content-Type", "applicatoin/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Basic " +
               Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("admin:123456")));
            request.AddObject(new
            {
                orderId = "123456",
                passgerName = "admin",
                type = "3",
                sign = "11ea1cf4f9ed1fd9e94f451963c90365"
            });

            IRestResponse response = client.Post(request);
            dynamic result = SimpleJson.DeserializeObject<dynamic>(response.Content);
            return result.access_token;
        }

        public static string GetUserToken()
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest("Token");
            request.AddHeader("Content-Type", "applicatoin/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Basic " +
               Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("admin:123456")));
            request.AddObject(new
            {
                grant_type = "password",
                username = "admin",
                password = "123456"
            });

            IRestResponse response = client.Post(request);
            dynamic result = SimpleJson.DeserializeObject<dynamic>(response.Content);
            return result.access_token;
        }


        public static void GetUser(string token)
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest("api/user");
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));
            IRestResponse response = client.Get(request);
            Console.WriteLine(response.Content);
        }
    }

    public class Sql
    {
        public Sql()
        {
            string a = @"select a.username,a.outdate,case isnull(b.cash,0) 
                when 0 then (select top 1 cash from t where t.username=a.username and t.outdate<a.outdate order by t.outdate desc)
                else b.cash
                end as cash,
                from a left join 
                (select username,outdate,min(cash) as cash from t group by username,outdate) b 
                on a.username=b.username";

            a = @"select t.year ,
                (select amount from t a where a.month=1 and a.year=t.year) as m1,
                (select amount from t a where a.month=2 and a.year=t.year) as m2,
                (select amount from t a where a.month=3 and a.year=t.year) as m3,
                from t group by t.year";
        }
    }
}
