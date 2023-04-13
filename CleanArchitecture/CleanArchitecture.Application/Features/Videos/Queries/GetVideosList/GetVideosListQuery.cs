using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;

public class GetVideosListQuery : IRequest<List<VideosVm>>
{
     public string? Username { get; set; }

     public GetVideosListQuery(string username)
     {
          Username = username ?? throw new ArgumentNullException(nameof(username));
     }
}