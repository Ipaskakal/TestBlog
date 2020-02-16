
using AutoMapper;
using Blog.WEB.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Microsoft.AspNetCore.Identity.IdentityUser, UserViewModel>();
            CreateMap<DAL.Models.Post, ListPostItemViewModel>();
            CreateMap<DAL.Models.Post, PostCreationViewModel>();
            CreateMap<DAL.Models.Comment, CommentViewModel>();
            CreateMap<DAL.Models.Post, PostViewModel>();
            CreateMap<PostCreationViewModel, DAL.Models.Post>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ListPostItemViewModel, DAL.Models.Post>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}