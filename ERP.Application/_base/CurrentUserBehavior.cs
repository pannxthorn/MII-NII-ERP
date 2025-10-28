using ERP.Shared._base;
using MediatR;

namespace ERP.Application._base
{
    /// <summary>
    /// MediatR Pipeline Behavior สำหรับ inject CurrentUser อัตโนมัติ
    /// ก่อนที่ Command/Query จะถูก execute
    /// </summary>
    public class CurrentUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserBehavior(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // ถ้า request implement ICurrentUserRequest ให้ inject CurrentUser
            if (request is ICurrentUserRequest currentUserRequest)
            {
                currentUserRequest.CurrentUser = _currentUserService.GetCurrentUser();
            }

            // ดำเนินการต่อไปยัง handler
            return await next();
        }
    }
}
