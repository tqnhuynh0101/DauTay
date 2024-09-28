using AutoMapper;
using DauTay.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DatPhat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpsController : ControllerBase
    {
        protected readonly IRepositoryWrapper _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;


        public HelpsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public HelpsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public HelpsController(IRepositoryWrapper repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
