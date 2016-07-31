using System;

using LPush.Model.Sample;
using LPush.Model;

namespace LPush.Service.Sample
{
    public interface IExampleService
    {
        /// Gets the example by identifier.
		/// </summary>
		/// <returns>The example by identifier.</returns>
		/// <param name="id">Identifier.</param>
		Example GetExampleById(int exampleId);

        /// <summary>
		/// Inserts the example.
		/// </summary>
		/// <returns><c>true</c>, if example was inserted, <c>false</c> otherwise.</returns>
		/// <param name="example">Example.</param>
		void InsertExample(Example example);


        /// <summary>
        /// Update a example
        /// </summary>
        /// <param name="example"></param>
        void UpdateExample(Example example);

        /// <summary>
        /// Delete a example
        /// </summary>
        /// <param name="id"></param>
        void DeleteExample(int id);
    }
}
