using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Application.TodoList.DTOs;

public record ProgressionDto(DateTime Date, decimal Percent);
