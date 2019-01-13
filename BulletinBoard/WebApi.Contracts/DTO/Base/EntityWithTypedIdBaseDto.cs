using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO.Base
{
    public abstract class EntityWithTypedIdBaseDto<TId>
    {
        /// <summary>
        /// Индентификатор
        /// </summary>
        public virtual TId Id { get; set; }

    }
}
