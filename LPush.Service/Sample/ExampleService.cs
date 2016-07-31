using System;

using LPush.Core.Data;
using LPush.Core.Caching;
using LPush.Model.Sample;


namespace LPush.Service.Sample
{
    public class ExampleService:IExampleService
    {
		/// <summary>
		/// Key for caching
		/// </summary>
		/// <remarks>
		/// {0} : example ID
		/// </remarks>
		private const string EXAMPLES_BY_ID_KEY = "LPush.example.id-{0}";

		private readonly IRepository<Example> exampleRepository =null;
        private readonly ICacheManager _cacheManager;
		//private readonly IEventPublisher _eventPublisher;
		public ExampleService(IRepository<Example> exampleRepository, ICacheManager cacheManager)
        {
            this.exampleRepository = exampleRepository;
            this._cacheManager = cacheManager;
        }
    

		/// <summary>
		/// Gets the example by identifier.
		/// </summary>
		/// <returns>The example by identifier.</returns>
		/// <param name="id">Identifier.</param>
		public virtual Example GetExampleById(int exampleId)
		{
            if (exampleId == 0)
                return null;

            string key = string.Format(EXAMPLES_BY_ID_KEY, exampleId);
            return _cacheManager.Get(key, () => exampleRepository.GetById(exampleId));
		}

		/// <summary>
		/// Inserts the example.
		/// </summary>
		/// <returns><c>true</c>, if example was inserted, <c>false</c> otherwise.</returns>
		/// <param name="example">Example.</param>
		public virtual void InsertExample(Example example)
		{
			if (example == null)
				throw new ArgumentNullException("example");

			exampleRepository.Insert(example);
		}

        /// <summary>
        /// Update a example
        /// </summary>
        /// <param name="example"></param>
        public virtual void UpdateExample(Example example)
        {
            if (example == null)
                throw new ArgumentNullException("example");
            //validate category hierarchy
            this.exampleRepository.Update(example);

            //cache
            string key = string.Format(EXAMPLES_BY_ID_KEY, example.Id);
            _cacheManager.Remove(key);
        }

        /// <summary>
        /// Delete a example
        /// </summary>
        /// <param name="id"></param>
        public virtual void DeleteExample(int id)
        {
            Example example = GetExampleById(id);
            if (example == null)
                throw new ArgumentNullException("id");

            example.Deleted = true;
            UpdateExample(example);
        }
    }

}
