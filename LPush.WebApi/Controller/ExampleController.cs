using System;
using System.Threading.Tasks;

using Nancy.Security;
using Nancy.ModelBinding;
using LPush.Service.Sample;
using LPush.Model.Sample;

using LPush.Core.Data;

namespace LPush.WebApi.Controller
{
    public class ExampleController: ApiModule
    {
        public ExampleController(IExampleService service) :base("Example")
        {
#if DEBUG
            this.RequiresAuthentication();
#endif
            Get["/{id}", true] = async (_, ct) =>
             {
                 DataResult<Example> result = new DataResult<Example>();
                 var example = await Task.Run(() =>
                 {
                     return service.GetExampleById(_.id);
                 });

                 result.Content = example;

                 return result;
             };

            Post["/",true] = async(_, ct) =>
            {
                DataResult<Example> result = new DataResult<Example>();
                Example example = this.Bind<Example>();
                example.CreateDt = DateTime.Now;
                example = await Task.Run(() => {
                    service.InsertExample(example);
                    return example;
                });

                result.Content = example;
                return result;

            };

            Put["/{year}"] = _ =>
            {
                var example = this.Bind<Example>();
                return new { Value = example.FirstName+ ",Hello World "+_.year };
            };

            Delete["/{name}"] = _ =>
            {
                return new { Value = _.name + ",Hello World" };
            };
        }
    }
}