using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPush.Core.Data
{
    
    public partial interface IRepository<T> 
	{
		/// <summary>
		/// Get entity by identifier
		/// </summary>
		/// <param name="id">Identifier</param>
		/// <returns>Entity</returns>
		T GetById(object id);

		/// <summary>
		/// Insert entity
		/// </summary>
		/// <param name="entity">Entity</param>
		void Insert(T entity);

		/// <summary>
		/// Insert entities
		/// </summary>
		/// <param name="entities">Entities</param>
		void Insert(IEnumerable<T> entities);

		/// <summary>
		/// Update entity
		/// </summary>
		/// <param name="entity">Entity</param>
		void Update(T entity);

		/// <summary>
		/// Update entities
		/// </summary>
		/// <param name="entities">Entities</param>
		void Update(IEnumerable<T> entities);

		/// <summary>
		/// Delete entity
		/// </summary>
		/// <param name="entity">Entity</param>
		void Delete(T entity);

		/// <summary>
		/// Delete entities
		/// </summary>
		/// <param name="entities">Entities</param>
		void Delete(IEnumerable<T> entities);
    }
}
