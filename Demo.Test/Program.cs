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

            //string url = "http://localhost:50291/api/QStore/SyncCompany";
            //string url = "http://localhost:50291/api/QStore/VisualCardExpend";

            //string url = "http://192.168.10.156:7921/api/QStore/SyncCompany";
            //string url = "http://192.168.10.156:7921/api/QStore/VisualCardExpend";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("companyId", "121&companyName=nuomi&totalMoney=10000&bnBatchId=209517&batchDetail%5B0%5D%5BbatchId%5D=209517&batchDetail%5B0%5D%5BbatchMoney%5D=10000&batchDetail%5B0%5D%5BcardDetail%5D=%5B%7B%22tailData%22%3A%5B4%5D%2C%22startId%22%3A%22261889000001995%22%2C%22endId%22%3A%22261889000001995%22%2C%22price%22%3A5000%7D%2C%7B%22tailData%22%3A%5B2%5D%2C%22startId%22%3A%22261889000001996%22%2C%22endId%22%3A%22261889000001996%22%2C%22price%22%3A5000%7D%5D&timestamp=1470386789&logid=2788026615&mac=99d17540639a00a758345ea0ee32df6a");
            
            //dic.Add("companyId", "119&companyName=%E8%A3%95%E7%A6%8F%E9%9B%86%E5%9B%A2&mac=bfaf4d0fe32191091f480f786dd0a543");

            //dic.Add("companyId", "50");
            //dic.Add("companyName", "test");
            //dic.Add("totalMoney", "123");
            //dic.Add("bnBatchId", "123456&batchDetail%5B0%5D%5BbatchId%5D=11&batchDetail%5B0%5D%5BbatchMoney%5D=100&batchDetail%5B0%5D%5BcardDetail%5D=%5B%7B%22startId%22%3A100%2C%22endId%22%3A105%2C%22price%22%3A400%2C%22tailData%22%3A%5B1%2C3%2C4%2C4%2C4%5D%7D%5D");
            //dic.Add("batchDetail", SimpleJson.SerializeObject(new List<dynamic>
            //{
            //    new
            //    {
            //        BatchId ="12345",
            //        BatchMoney = 123,
            //        CardDetail = new List<dynamic>()
            //        {
            //            new { StartId="1",EndId ="2",Price =123 }
            //        },
            //    },
            //    //new
            //    //{   BatchId="12349",
            //    //    BatchMoney =123,
            //    //    CardDetail=new List<dynamic>()
            //    //    {
            //    //        new { StartId="2",EndId ="3",Price =123}
            //    //    },
            //    //},
            //}));


            HttpWebResponse response = HttpWebUtils.Post(url, dic);

            using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            Console.ReadLine();
        }

        public static string UrlDeCode(string str, Encoding encoding=null)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                    
                string code = System.Web.HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = System.Web.HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return System.Web.HttpUtility.UrlDecode(str, encoding);
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
