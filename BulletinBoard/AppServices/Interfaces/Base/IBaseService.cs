using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Contracts.DTO;
using WebApi.Contracts.DTO.Base;

namespace AppServices.Interfaces.Base
{
    public interface IBaseService<T>  where T : EntityWithTypedIdBaseDto<T>
    {
        IQueryable<T> GetAll();
        T Get();

    }
}
