using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WebApplication.Site.AppStart;
using WebApplication.Site.Models.DTO;
using WebApplication.Domain;
using WebApplication.Data;

namespace WebApplication.Site.Controllers
{
    [Route("api/[controller]")]
    public class TodoController
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodoController(
            ITodoRepository todoRepository,
            IMapper mapper
        ) {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public List<TodoDTO> GetAll()
        {
            IEnumerable<Todo> todos = _todoRepository.All.ToList();

            List<TodoDTO> todosDto = _mapper.Map<IEnumerable<TodoDTO>>(todos).ToList();
            return todosDto;
        }

        [HttpGet("{id}")]
        public TodoDTO Get(Guid id)
        {
            Todo todo =  _todoRepository.Find(id);
            TodoDTO todoDTO = _mapper.Map<TodoDTO>(todo);

            return todoDTO;
        }
        
    }
}