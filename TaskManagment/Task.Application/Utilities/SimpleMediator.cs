using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Exceptions;


namespace TaskManagment.Application.Utilities
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            await ApplyValidations(request);
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new MediatorException($"Handler was not found for {request.GetType().Name}");
            }

            var method = handlerType.GetMethod("Handle")!;
            return await (Task<TResponse>)method.Invoke(handler, new Object[] { request })!;
        }

        public async Task Send(IRequest request)
        {
            await ApplyValidations(request);
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
            var handler = serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new MediatorException($"Handler was not found for {request.GetType().Name}");
            }
            var method = handlerType.GetMethod("Handle")!;
            await (Task)method.Invoke(handler, new Object[] { request })!;
        }

        private async Task ApplyValidations(object request)
        {
            var valdatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
            var validator = serviceProvider.GetService(valdatorType);
            if (validator is not null)
            {
                var validateMethod = valdatorType.GetMethod("ValidateAsync");
                var taskToValidate = (Task)validateMethod!.Invoke(validator, new object[] { request, CancellationToken.None })!;
                await taskToValidate;
                var result = taskToValidate.GetType().GetProperty("Result");
                var validationResult = (ValidationResult)result!.GetValue(taskToValidate)!;
                if (!validationResult.IsValid)
                {
                    throw new CustomValidationException(validationResult);
                }
            }
        }
    }
}
