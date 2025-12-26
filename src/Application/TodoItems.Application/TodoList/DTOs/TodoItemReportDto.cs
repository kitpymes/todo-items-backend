using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Application.TodoList.DTOs;

public record TodoItemReportDto(
    int Id,
    string Title,
    string Description,
    string Category,
    List<ProgressionDto> History
);
