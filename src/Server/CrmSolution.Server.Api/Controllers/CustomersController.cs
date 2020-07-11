using Bit.Core.Exceptions;
using Bit.OData.ODataControllers;
using CrmSolution.Server.Model;
using CrmSolution.Shared.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrmSolution.Server.Api.Controllers
{
    public class CustomersController : DtoSetController<CustomerDto, Customer, int>
    {
        [Function]
        public int Sum(int n1, int n2)
        {
            if (n1 < 0 || n2 < 0)
                throw new BadRequestException(")-:");

            return n1 + n2;
        }

        public async override Task<IQueryable<CustomerDto>> GetAll(CancellationToken cancellationToken)
        {
            await Task.Delay(2000);

            return await base.GetAll(cancellationToken);
        }

        public async override Task<SingleResult<CustomerDto>> Create(CustomerDto dto, CancellationToken cancellationToken)
        {
            await Task.Delay(2000);

            return await base.Create(dto, cancellationToken);
        }

        public async override Task<SingleResult<CustomerDto>> Update(int key, CustomerDto dto, CancellationToken cancellationToken)
        {
            await Task.Delay(2000);

            return await base.Update(key, dto, cancellationToken);
        }

        public async override Task Delete(int key, CancellationToken cancellationToken)
        {
            await Task.Delay(2000);

            await base.Delete(key, cancellationToken);
        }
    }
}
