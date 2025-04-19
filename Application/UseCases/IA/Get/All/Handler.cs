using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Get.All;

public class Handler(
    IIARepository iARepository
) : IRequestHandler<Request, BaseResponse>
{
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
       var ia = await iARepository.GetAllProjectedAsync( 
            x => x.DeletedDate == null, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            },
            cancellationToken,
            request.skip,
            request.take,
            x => x.Picture 
        );

        if(ia is null || !ia.Any()) return new BaseResponse(404, "IA not found");
        return new BaseResponse(200, "IA found", null, ia);
    }
}
