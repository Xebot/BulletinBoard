using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinDomain.Entities.Base
{
    public abstract class EntityWithTypedIdBase<TId>
    {
        /// <summary>
        /// Индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TId Id { get; protected set; }
    }
}
