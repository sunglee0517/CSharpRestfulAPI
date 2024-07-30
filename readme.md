| Method | URL                                      |
|--------|------------------------------------------|
| GET    | http://localhost:5122/company/boards/list|
| GET    | http://localhost:5122/company/boards/detail/1|
| POST   | http://localhost:5122/company/boards/insert|
| POST   | http://localhost:5122/company/boards/update|
| POST   | http://localhost:5122/company/boards/delete|
| GET    | http://localhost:5122/company/qna/list   |
| GET    | http://localhost:5122/company/qna/detail/1|
| POST   | http://localhost:5122/company/qna/insert |
| POST   | http://localhost:5122/company/qna/answer |
| POST   | http://localhost:5122/company/qna/edit   |
| POST   | http://localhost:5122/company/qna/delete |
| GET    | http://localhost:5122/company/dataroom/list|
| GET    | http://localhost:5122/company/dataroom/detail/1|
| POST   | http://localhost:5122/company/dataroom/upload|
| POST   | http://localhost:5122/company/dataroom/update|
| POST   | http://localhost:5122/company/dataroom/delete|
| GET    | http://localhost:5122/company/products/list|
| GET    | http://localhost:5122/company/products/detail/1|
| POST   | http://localhost:5122/company/products/insert|
| POST   | http://localhost:5122/company/products/update|
| POST   | http://localhost:5122/company/products/delete|
| GET    | http://localhost:5122/company/members/getMemberList|
| GET    | http://localhost:5122/company/members/getMember/alice|
| GET    | http://localhost:5122/company/members/mypage/alice|
| POST   | http://localhost:5122/company/members/join |
| POST   | http://localhost:5122/company/members/myInfoEdit|
| POST   | http://localhost:5122/company/members/login|
| POST   | http://localhost:5122/company/members/logout|
| POST   | http://localhost:5122/company/email/send   |



# 1. Visual Studio 개발 환경 설정

## 1-1. Visual Studio 설치

### 1-1-1. Visual Studio 다운로드 및 설치

1. Visual Studio 홈페이지에서 Visual Studio Community, Professional, 또는 Enterprise 버전을 다운로드합니다.
2. 설치 시, .NET Desktop Development 및 ASP.NET and web development 워크로드를 선택합니다.

<br><br><br>

# 2. 새로운 ASP.NET Core 프로젝트 생성

## 2-1. Visual Studio 열기

- Visual Studio를 실행합니다.

<br><br>

## 2-2. 새 프로젝트 만들기

- Create a new project를 클릭합니다.

<br><br>

## 2-3. 프로젝트 템플릿 선택

- ASP.NET Core Web Application 템플릿을 선택하고 Next를 클릭합니다.

<br><br>

## 2-4. 프로젝트 이름 지정:

1. 프로젝트 이름을 입력합니다. 예: YourProject.
2. 위치를 지정하고 Create를 클릭합니다.

<br><br>

## 2-5. 프로젝트 템플릿 구성

- API를 선택하고 Create를 클릭합니다.

<br><br><br>

# 3. 프로젝트 설정

## 3-1. 패키지 관리자 콘솔 사용

- Tools > NuGet Package Manager > Package Manager Console를 엽니다.

- 아래 명령을 실행하여 필요한 패키지를 설치합니다:

```sh
Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
Install-Package Swashbuckle.AspNetCore
Install-Package MailKit
Install-Package MySql.Data
```

```powershell
dotnet new webapi -n company
cd company
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Swashbuckle.AspNetCore
dotnet add package MailKit
dotnet add package MySql.Data
```

<br><br><br>

## 3-2. 프로젝트 파일 구성

- 컨트롤러, 서비스, 모델, DAO, 설정 파일 등을 생성하고 구성합니다.

<br><br>

### 3-2-1. Program.cs

```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace company
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
```

<br><br>

### 3-2-2. Startup.cs

```csharp
using company.Controllers;
using company.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace company
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<BoardDAO>(provider => new BoardDAO(connectionString));
            services.AddScoped<BoardService>();
            // services.AddScoped<BoardController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<QnaDAO>(provider => new QnaDAO(connectionString));
            services.AddScoped<QnaService>();
            // services.AddScoped<QnaController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<DataroomDAO>(provider => new DataroomDAO(connectionString));
            services.AddScoped<DataroomService>();
            // services.AddScoped<DataroomController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<ProductDAO>(provider => new ProductDAO(connectionString));
            services.AddScoped<ProductService>();
            // services.AddScoped<ProductController>(); // Controller는 여기서 추가할 필요 없음

            services.AddScoped<MemberDAO>(provider => new MemberDAO(connectionString));
            services.AddScoped<MemberService>();
            // services.AddScoped<MemberController>(); // Controller는 여기서 추가할 필요 없음

            services.AddControllers();
            services.AddTransient<EmailService>();
            services.AddScoped<EmailService>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

<br><br>

### 3-2-3. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3307;Database=company;Uid=root;Pwd=1234;"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@example.com",
    "SmtpPass": "your-email-password",
    "FromName": "Your App",
    "FromEmail": "your-email@example.com"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```


<br><br>

### 3-2-4. company.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

	<PropertyGroup>
		<TargetFramework>net8.0</TargetFramework>
	</PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="8.0.0" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
		<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
		<PackageReference Include="MailKit" Version="3.2.0" />
		<PackageReference Include="MySql.Data" Version="8.0.27" />
	</ItemGroup>

</Project>
```

<br><br><br>

# 4. 코드 작성 및 테스트

- 각각의 Controller, Service, DAO, Model 클래스를 작성하고 프로젝트 구조에 맞게 배치합니다. 
- 모든 파일을 작성한 후 프로젝트를 빌드하고 실행하여 API 엔드포인트를 테스트합니다.

```bash
dotnet build
dotnet run
```

브라우저에서 http://localhost:5122/swagger로 이동하여 Swagger UI를 통해 API를 테스트할 수 있습니다.

<br><br><br>

# 5. 프로젝트 구조

```lua
company/
│
├── Controllers/
│   ├── BoardController.cs
│   ├── QnaController.cs
│   ├── DataroomController.cs
│   ├── ProductController.cs
│   ├── MemberController.cs
│   ├── EmailController.cs
│   └── HomeController.cs
│
├── Models/
│   ├── Board.cs
│   ├── Qna.cs
│   ├── Dataroom.cs
│   ├── Product.cs
│   ├── Member.cs
│   └── EmailRequest.cs
│
├── Services/
│   ├── BoardService.cs
│   ├── QnaService.cs
│   ├── DataroomService.cs
│   ├── ProductService.cs
│   ├── MemberService.cs
│   └── EmailService.cs
│
├── DataAccess/
│   ├── BoardDAO.cs
│   ├── QnaDAO.cs
│   ├── DataroomDAO.cs
│   ├── ProductDAO.cs
│   └── MemberDAO.cs
│
├── appsettings.json
├── Program.cs
├── Startup.cs
└── company.csproj
```

<br><br><br>

# 6. 메인 컨트롤러 및 Board 관련 클래스 작성

## 6-1. Controllers/HomeController.cs

```csharp
using Microsoft.AspNetCore.Mvc;

namespace company.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET: /Home
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to the Company API!");
        }
    }
}
```

<br><br><br>

## 6-2. Models/Board DTO 클래스

- Board 테이블과 매핑되는 DTO 클래스를 정의합니다.

```csharp
public class Board
{
    public int No { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime ResDate { get; set; }
    public int Hits { get; set; }
}
```

<br><br><br>

## 6-3. DataAccess/BoardDAO 클래스

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BoardDAO
{
    private readonly string _connectionString;

    public BoardDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Board>> GetAllBoardsAsync()
    {
        List<Board> boards = new List<Board>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM board";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        boards.Add(new Board
                        {
                            No = Convert.ToInt32(reader["no"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return boards;
    }

    public async Task<Board> GetBoardByIdAsync(int id)
    {
        Board board = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM board WHERE no = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        board = new Board
                        {
                            No = Convert.ToInt32(reader["no"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return board;
    }

    public async Task<Board> InsertBoardAsync(Board board)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO board (title, content, author, resdate, hits) 
                             VALUES (@title, @content, @author, 
                                     @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", board.Title);
                command.Parameters.AddWithValue("@content", board.Content);
                command.Parameters.AddWithValue("@author", board.Author);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                board.No = insertedId; // Set the ID of the inserted board
            }
        }

        return board;
    }

    public async Task<bool> UpdateBoardAsync(Board board)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE board 
                             SET title = @title, 
                                 content = @content, 
                                 author = @author,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE no = @no";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", board.Title);
                command.Parameters.AddWithValue("@content", board.Content);
                command.Parameters.AddWithValue("@author", board.Author);
                command.Parameters.AddWithValue("@resdate", board.ResDate);
                command.Parameters.AddWithValue("@hits", board.Hits);
                command.Parameters.AddWithValue("@no", board.No);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteBoardAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM board 
                             WHERE no = @no";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@no", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
```

<br>

주의사항 및 추가 설명

1. Connection String 설정: BoardDAO 클래스의 생성자에서 주어진 _connectionString을 사용하여 데이터베이스에 연결합니다. 이는 MariaDB 데이터베이스의 연결 정보를 포함해야 합니다.

2. 비동기 메서드 사용: 모든 데이터베이스 작업은 비동기 메서드로 구현되었습니다(Async 접미사가 있는 메서드들). 이는 서버의 성능을 향상시키고, 동시성을 처리하는데 유리합니다.

3. SQL Injection 방지: MySqlCommand의 Parameters.AddWithValue 메서드를 사용하여 SQL Injection 공격을 방지하도록 합니다.

4. 트랜잭션 처리: 예를 들어 여러 쿼리가 한 번에 실행되어야 할 경우, MySqlTransaction을 사용하여 트랜잭션을 관리할 수 있습니다.

5. 오류 처리: 각 메서드에서 발생할 수 있는 예외 상황에 대한 적절한 예외 처리를 추가해야 합니다.


<br><br><br>

## 6-4. Services/BoardService 클래스

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

public class BoardService
{
    private readonly BoardDAO _boardDAO;

    public BoardService(BoardDAO boardDAO)
    {
        _boardDAO = boardDAO;
    }

    public async Task<List<Board>> GetAllBoardsAsync()
    {
        return await _boardDAO.GetAllBoardsAsync();
    }

    public async Task<Board> GetBoardByIdAsync(int id)
    {
        return await _boardDAO.GetBoardByIdAsync(id);
    }

    public async Task<Board> InsertBoardAsync(Board board)
    {
        return await _boardDAO.InsertBoardAsync(board);
    }

    public async Task<bool> UpdateBoardAsync(Board board)
    {
        return await _boardDAO.UpdateBoardAsync(board);
    }

    public async Task<bool> DeleteBoardAsync(int id)
    {
        return await _boardDAO.DeleteBoardAsync(id);
    }
}
```

<br><br><br>

## 6-5. Controllers/BoardsController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardsController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Board>>> GetAllBoards()
        {
            var boards = await _boardService.GetAllBoardsAsync();
            return Ok(boards);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Board>> GetBoardById(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return Ok(board);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Board>> InsertBoard(Board board)
        {
            var insertedBoard = await _boardService.InsertBoardAsync(board);
            return CreatedAtAction(nameof(GetBoardById), new { id = insertedBoard.No }, insertedBoard);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateBoard(Board board)
        {
            var success = await _boardService.UpdateBoardAsync(board);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var success = await _boardService.DeleteBoardAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
```

<br><br><br>

# 7. Qna 관련 클래스 작성

## 7-1. Models/Qna DTO 클래스

- Qna 테이블과 매핑되는 DTO 클래스를 정의합니다.

```csharp
public class Qna
{
    public int Qno { get; set; }
    public int Lev { get; set; }
    public int? Parno { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime ResDate { get; set; }
    public int Hits { get; set; }
}
```

<br><br><br>

## 7-2. DataAccess/QnaDAO 클래스

- Qna 데이터베이스 접근 객체(DAO)를 구현합니다.

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QnaDAO
{
    private readonly string _connectionString;

    public QnaDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Qna>> GetAllQnasAsync()
    {
        List<Qna> qnas = new List<Qna>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM qna";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        qnas.Add(new Qna
                        {
                            Qno = Convert.ToInt32(reader["qno"]),
                            Lev = Convert.ToInt32(reader["lev"]),
                            Parno = reader["parno"] != DBNull.Value ? Convert.ToInt32(reader["parno"]) : (int?)null,
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return qnas;
    }

    public async Task<Qna> GetQnaByIdAsync(int id)
    {
        Qna qna = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM qna WHERE qno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        qna = new Qna
                        {
                            Qno = Convert.ToInt32(reader["qno"]),
                            Lev = Convert.ToInt32(reader["lev"]),
                            Parno = reader["parno"] != DBNull.Value ? Convert.ToInt32(reader["parno"]) : (int?)null,
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return qna;
    }

    public async Task<Qna> InsertQnaAsync(Qna qna)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO qna (lev, parno, title, content, author, resdate, hits) 
                             VALUES (@lev, @parno, @title, @content, @author,
                                     @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@lev", qna.Lev);
                command.Parameters.AddWithValue("@parno", qna.Parno ?? (object)DBNull.Value); // Handle nullable parno
                command.Parameters.AddWithValue("@title", qna.Title);
                command.Parameters.AddWithValue("@content", qna.Content);
                command.Parameters.AddWithValue("@author", qna.Author);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                qna.Qno = insertedId; // Set the ID of the inserted qna
            }
        }

        return qna;
    }

    public async Task<bool> UpdateQnaAsync(Qna qna)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE qna 
                             SET lev = @lev, 
                                 parno = @parno,
                                 title = @title, 
                                 content = @content, 
                                 author = @author,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE qno = @qno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@lev", qna.Lev);
                command.Parameters.AddWithValue("@parno", qna.Parno ?? (object)DBNull.Value); // Handle nullable parno
                command.Parameters.AddWithValue("@title", qna.Title);
                command.Parameters.AddWithValue("@content", qna.Content);
                command.Parameters.AddWithValue("@author", qna.Author);
                command.Parameters.AddWithValue("@resdate", qna.ResDate);
                command.Parameters.AddWithValue("@hits", qna.Hits);
                command.Parameters.AddWithValue("@qno", qna.Qno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteQnaAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM qna 
                             WHERE qno = @qno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@qno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
```

<br><br><br>

## 7-3. Services/QnaService 클래스

- Qna 비즈니스 로직을 처리하는 서비스 클래스를 구현합니다.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

public class QnaService
{
    private readonly QnaDAO _qnaDAO;

    public QnaService(QnaDAO qnaDAO)
    {
        _qnaDAO = qnaDAO;
    }

    public async Task<List<Qna>> GetAllQnasAsync()
    {
        return await _qnaDAO.GetAllQnasAsync();
    }

    public async Task<Qna> GetQnaByIdAsync(int id)
    {
        return await _qnaDAO.GetQnaByIdAsync(id);
    }

    public async Task<Qna> InsertQnaAsync(Qna qna)
    {
        return await _qnaDAO.InsertQnaAsync(qna);
    }

    public async Task<bool> UpdateQnaAsync(Qna qna)
    {
        return await _qnaDAO.UpdateQnaAsync(qna);
    }

    public async Task<bool> DeleteQnaAsync(int id)
    {
        return await _qnaDAO.DeleteQnaAsync(id);
    }
}
```

<br><br><br>

## 7-4. Controllers/QnaController 클래스

- HTTP 요청을 처리하고 QnaService를 호출하여 데이터를 반환합니다.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class QnaController : ControllerBase
    {
        private readonly QnaService _qnaService;

        public QnaController(QnaService qnaService)
        {
            _qnaService = qnaService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Qna>>> GetAllQnas()
        {
            var qnas = await _qnaService.GetAllQnasAsync();
            return Ok(qnas);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Qna>> GetQnaById(int id)
        {
            var qna = await _qnaService.GetQnaByIdAsync(id);
            if (qna == null)
            {
                return NotFound();
            }
            return Ok(qna);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Qna>> InsertQna(Qna qna)
        {
            var insertedQna = await _qnaService.InsertQnaAsync(qna);
            return CreatedAtAction(nameof(GetQnaById), new { id = insertedQna.Qno }, insertedQna);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateQna(Qna qna)
        {
            var success = await _qnaService.UpdateQnaAsync(qna);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteQna(int id)
        {
            var success = await _qnaService.DeleteQnaAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
```

<br>

**추가 사항**

1. 의존성 주입: Startup.cs에서 서비스와 데이터 접근 객체(DAO)를 등록하여 의존성 주입을 설정해야 합니다.
2. 예외 처리: 예외 상황에 대한 적절한 처리를 추가해야 합니다(예: 데이터베이스 연결 실패, 쿼리 실패 등).
3. 보안 고려 사항: 입력 데이터의 유효성 검사, SQL Injection 방어 등을 고려하여 보안을 강화해야 합니다.

<br><br><br>

# 8. Dataroom 관련 클래스 작성

## 8-1. Models/Dataroom DTO 클래스

- Dataroom 테이블과 매핑되는 DTO 클래스를 정의합니다.

```csharp
public class Dataroom
{
    public int Dno { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string DataFile { get; set; }
    public DateTime ResDate { get; set; }
    public int Hits { get; set; }
}
```

<br><br><br>

## 8-2. DataAccess/DataroomDAO 클래스

- Dataroom 데이터베이스 접근 객체(DAO)를 구현합니다.

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataroomDAO
{
    private readonly string _connectionString;

    public DataroomDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Dataroom>> GetAllDataroomsAsync()
    {
        List<Dataroom> datarooms = new List<Dataroom>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM dataroom";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        datarooms.Add(new Dataroom
                        {
                            Dno = Convert.ToInt32(reader["dno"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            DataFile = reader["datafile"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return datarooms;
    }

    public async Task<Dataroom> GetDataroomByIdAsync(int id)
    {
        Dataroom dataroom = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM dataroom WHERE dno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dataroom = new Dataroom
                        {
                            Dno = Convert.ToInt32(reader["dno"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            DataFile = reader["datafile"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return dataroom;
    }

    public async Task<Dataroom> InsertDataroomAsync(Dataroom dataroom)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO dataroom (title, content, author, datafile, resdate, hits) 
                             VALUES (@title, @content, @author,
                                     @datafile, @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", dataroom.Title);
                command.Parameters.AddWithValue("@content", dataroom.Content);
                command.Parameters.AddWithValue("@author", dataroom.Author);
                command.Parameters.AddWithValue("@datafile", dataroom.DataFile);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                dataroom.Dno = insertedId; // Set the ID of the inserted dataroom
            }
        }

        return dataroom;
    }

    public async Task<bool> UpdateDataroomAsync(Dataroom dataroom)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE dataroom 
                             SET title = @title, 
                                 content = @content, 
                                 author = @author,
                                 datafile = @datafile,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE dno = @dno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", dataroom.Title);
                command.Parameters.AddWithValue("@content", dataroom.Content);
                command.Parameters.AddWithValue("@author", dataroom.Author);
                command.Parameters.AddWithValue("@datafile", dataroom.DataFile);
                command.Parameters.AddWithValue("@resdate", dataroom.ResDate);
                command.Parameters.AddWithValue("@hits", dataroom.Hits);
                command.Parameters.AddWithValue("@dno", dataroom.Dno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteDataroomAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM dataroom 
                             WHERE dno = @dno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
```

<br><br><br>

## 8-3. Services/DataroomService 클래스

- Dataroom 비즈니스 로직을 처리하는 서비스 클래스를 구현합니다.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataroomService
{
    private readonly DataroomDAO _dataroomDAO;

    public DataroomService(DataroomDAO dataroomDAO)
    {
        _dataroomDAO = dataroomDAO;
    }

    public async Task<List<Dataroom>> GetAllDataroomsAsync()
    {
        return await _dataroomDAO.GetAllDataroomsAsync();
    }

    public async Task<Dataroom> GetDataroomByIdAsync(int id)
    {
        return await _dataroomDAO.GetDataroomByIdAsync(id);
    }

    public async Task<Dataroom> InsertDataroomAsync(Dataroom dataroom)
    {
        return await _dataroomDAO.InsertDataroomAsync(dataroom);
    }

    public async Task<bool> UpdateDataroomAsync(Dataroom dataroom)
    {
        return await _dataroomDAO.UpdateDataroomAsync(dataroom);
    }

    public async Task<bool> DeleteDataroomAsync(int id)
    {
        return await _dataroomDAO.DeleteDataroomAsync(id);
    }
}
```

<br><br><br>

## 8-4. Controllers/DataroomController 클래스

- HTTP 요청을 처리하고 DataroomService를 호출하여 데이터를 반환합니다.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class DataroomController : ControllerBase
    {
        private readonly DataroomService _dataroomService;

        public DataroomController(DataroomService dataroomService)
        {
            _dataroomService = dataroomService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Dataroom>>> GetAllDatarooms()
        {
            var datarooms = await _dataroomService.GetAllDataroomsAsync();
            return Ok(datarooms);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Dataroom>> GetDataroomById(int id)
        {
            var dataroom = await _dataroomService.GetDataroomByIdAsync(id);
            if (dataroom == null)
            {
                return NotFound();
            }
            return Ok(dataroom);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Dataroom>> UploadDataroom(Dataroom dataroom)
        {
            var insertedDataroom = await _dataroomService.InsertDataroomAsync(dataroom);
            return CreatedAtAction(nameof(GetDataroomById), new { id = insertedDataroom.Dno }, insertedDataroom);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateDataroom(Dataroom dataroom)
        {
            var success = await _dataroomService.UpdateDataroomAsync(dataroom);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteDataroom(int id)
        {
            var success = await _dataroomService.DeleteDataroomAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
```

<br>

**추가 사항**

1. 의존성 주입: Startup.cs에서 서비스와 데이터 접근 객체(DAO)를 의존성 주입해야 합니다.
2. 예외 처리: 데이터베이스 접근 중 발생할 수 있는 예외 상황에 대한 적절한 처리를 추가해야 합니다.
3. 보안 고려 사항: 입력 데이터의 유효성 검사와 SQL Injection 방어를 위한 방법을 고려하여 보안을 강화해야 합니다.

<br><br><br>

# 9. Product 관련 클래스

## 9-1. Models/Product DTO 클래스

- Product 테이블과 매핑되는 DTO 클래스를 정의합니다.

```csharp
public class Product
{
    public int Pno { get; set; }
    public string Cate { get; set; }
    public string Pname { get; set; }
    public string Pcontent { get; set; }
    public string Img1 { get; set; }
    public string Img2 { get; set; }
    public string Img3 { get; set; }
    public DateTime ResDate { get; set; }
    public int Hits { get; set; }
}
```

<br><br>

## 9-2. DataAccess/ProductDAO 클래스

- Product 데이터베이스 접근 객체(DAO)를 구현합니다.

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductDAO
{
    private readonly string _connectionString;

    public ProductDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        List<Product> products = new List<Product>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM product";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Pno = Convert.ToInt32(reader["pno"]),
                            Cate = reader["cate"].ToString(),
                            Pname = reader["pname"].ToString(),
                            Pcontent = reader["pcontent"].ToString(),
                            Img1 = reader["img1"].ToString(),
                            Img2 = reader["img2"].ToString(),
                            Img3 = reader["img3"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return products;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        Product product = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM product WHERE pno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Pno = Convert.ToInt32(reader["pno"]),
                            Cate = reader["cate"].ToString(),
                            Pname = reader["pname"].ToString(),
                            Pcontent = reader["pcontent"].ToString(),
                            Img1 = reader["img1"].ToString(),
                            Img2 = reader["img2"].ToString(),
                            Img3 = reader["img3"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return product;
    }

    public async Task<Product> InsertProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO product (cate, pname, pcontent, img1, img2, img3, resdate, hits) 
                             VALUES (@cate, @pname, @pcontent, @img1, @img2, @img3, @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@cate", product.Cate);
                command.Parameters.AddWithValue("@pname", product.Pname);
                command.Parameters.AddWithValue("@pcontent", product.Pcontent);
                command.Parameters.AddWithValue("@img1", product.Img1);
                command.Parameters.AddWithValue("@img2", product.Img2);
                command.Parameters.AddWithValue("@img3", product.Img3);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                product.Pno = insertedId; // Set the ID of the inserted product
            }
        }

        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE product 
                             SET cate = @cate, 
                                 pname = @pname, 
                                 pcontent = @pcontent,
                                 img1 = @img1,
                                 img2 = @img2,
                                 img3 = @img3,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE pno = @pno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@cate", product.Cate);
                command.Parameters.AddWithValue("@pname", product.Pname);
                command.Parameters.AddWithValue("@pcontent", product.Pcontent);
                command.Parameters.AddWithValue("@img1", product.Img1);
                command.Parameters.AddWithValue("@img2", product.Img2);
                command.Parameters.AddWithValue("@img3", product.Img3);
                command.Parameters.AddWithValue("@resdate", product.ResDate);
                command.Parameters.AddWithValue("@hits", product.Hits);
                command.Parameters.AddWithValue("@pno", product.Pno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM product 
                             WHERE pno = @pno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
```

<br><br>

## 9-3. Services/ProductService 클래스

- Product 비즈니스 로직을 처리하는 서비스 클래스를 구현합니다.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly ProductDAO _productDAO;

    public ProductService(ProductDAO productDAO)
    {
        _productDAO = productDAO;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productDAO.GetAllProductsAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productDAO.GetProductByIdAsync(id);
    }

    public async Task<Product> InsertProductAsync(Product product)
    {
        return await _productDAO.InsertProductAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        return await _productDAO.UpdateProductAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productDAO.DeleteProductAsync(id);
    }
}
```

<br><br>

## 9-4. Controllers/ProductController 클래스

- HTTP 요청을 처리하고 ProductService를 호출하여 데이터를 반환합니다.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Product>> InsertProduct(Product product)
        {
            var insertedProduct = await _productService.InsertProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = insertedProduct.Pno }, insertedProduct);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var success = await _productService.UpdateProductAsync(product);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
```

<br>

**추가 사항**

1. 의존성 주입: Startup.cs에서 ProductService와 ProductDAO를 의존성 주입해야 합니다.
2. 예외 처리: 데이터베이스 접근 중 발생할 수 있는 예외 상황에 대한 적절한 처리를 추가해야 합니다.
3. 보안 고려 사항: 입력 데이터의 유효성 검사와 SQL Injection 방어를 위한 방법을 고려하여 보안을 강화해야 합니다.

<br><br><br>

# 10. Member 관련 클래스

## 10-1. Models/Member DTO 클래스

- Member 테이블과 매핑되는 DTO 클래스를 정의합니다.

```csharp
public class Member
{
    public string Id { get; set; }
    public string Pw { get; set; }
    public string Name { get; set; }
    public DateTime Birth { get; set; }
    public string Email { get; set; }
    public string Tel { get; set; }
    public string Addr1 { get; set; }
    public string Addr2 { get; set; }
    public string Postcode { get; set; }
    public DateTime RegDate { get; set; }
}
```

<br><br>

## 10-2. DataAccess/MemberDAO 클래스

- Member 데이터베이스 접근 객체(DAO)를 구현합니다.

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberDAO
{
    private readonly string _connectionString;

    public MemberDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        List<Member> members = new List<Member>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM member";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        members.Add(new Member
                        {
                            Id = reader["id"].ToString(),
                            Pw = reader["pw"].ToString(),
                            Name = reader["name"].ToString(),
                            Birth = Convert.ToDateTime(reader["birth"]),
                            Email = reader["email"].ToString(),
                            Tel = reader["tel"].ToString(),
                            Addr1 = reader["addr1"].ToString(),
                            Addr2 = reader["addr2"].ToString(),
                            Postcode = reader["postcode"].ToString(),
                            RegDate = Convert.ToDateTime(reader["regdate"])
                        });
                    }
                }
            }
        }

        return members;
    }

    public async Task<Member> GetMemberByIdAsync(string id)
    {
        Member member = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM member WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        member = new Member
                        {
                            Id = reader["id"].ToString(),
                            Pw = reader["pw"].ToString(),
                            Name = reader["name"].ToString(),
                            Birth = Convert.ToDateTime(reader["birth"]),
                            Email = reader["email"].ToString(),
                            Tel = reader["tel"].ToString(),
                            Addr1 = reader["addr1"].ToString(),
                            Addr2 = reader["addr2"].ToString(),
                            Postcode = reader["postcode"].ToString(),
                            RegDate = Convert.ToDateTime(reader["regdate"])
                        };
                    }
                }
            }
        }

        return member;
    }

    public async Task<Member> InsertMemberAsync(Member member)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO member (id, pw, name, birth, email, tel, addr1, addr2, postcode, regdate) 
                             VALUES (@id, @pw, @name, @birth, @email, @tel, @addr1, @addr2, @postcode, @regdate)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", member.Id);
                command.Parameters.AddWithValue("@pw", member.Pw);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@birth", member.Birth);
                command.Parameters.AddWithValue("@email", member.Email);
                command.Parameters.AddWithValue("@tel", member.Tel);
                command.Parameters.AddWithValue("@addr1", member.Addr1);
                command.Parameters.AddWithValue("@addr2", member.Addr2);
                command.Parameters.AddWithValue("@postcode", member.Postcode);
                command.Parameters.AddWithValue("@regdate", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
        }

        return member;
    }

    public async Task<bool> UpdateMemberAsync(Member member)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE member 
                             SET pw = @pw, 
                                 name = @name, 
                                 birth = @birth,
                                 email = @email,
                                 tel = @tel,
                                 addr1 = @addr1,
                                 addr2 = @addr2,
                                 postcode = @postcode
                             WHERE id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pw", member.Pw);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@birth", member.Birth);
                command.Parameters.AddWithValue("@email", member.Email);
                command.Parameters.AddWithValue("@tel", member.Tel);
                command.Parameters.AddWithValue("@addr1", member.Addr1);
                command.Parameters.AddWithValue("@addr2", member.Addr2);
                command.Parameters.AddWithValue("@postcode", member.Postcode);
                command.Parameters.AddWithValue("@id", member.Id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM member 
                             WHERE id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
```
<br><br>

## 10-3. Services/MemberService 클래스

- Member 비즈니스 로직을 처리하는 서비스 클래스를 구현합니다.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberService
{
    private readonly MemberDAO _memberDAO;

    public MemberService(MemberDAO memberDAO)
    {
        _memberDAO = memberDAO;
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        return await _memberDAO.GetAllMembersAsync();
    }

    public async Task<Member> GetMemberByIdAsync(string id)
    {
        return await _memberDAO.GetMemberByIdAsync(id);
    }

    public async Task<Member> InsertMemberAsync(Member member)
    {
        // Additional business logic can be added here, such as password hashing

        return await _memberDAO.InsertMemberAsync(member);
    }

    public async Task<bool> UpdateMemberAsync(Member member)
    {
        // Additional business logic can be added here

        return await _memberDAO.UpdateMemberAsync(member);
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        return await _memberDAO.DeleteMemberAsync(id);
    }
}
```
<br><br>

## 10-4. Controllers/MemberController 클래스

- HTTP 요청을 처리하고 MemberService를 호출하여 데이터를 반환합니다.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("getMemberList")]
        public async Task<ActionResult<List<Member>>> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        [HttpGet("getMember/{id}")]
        public async Task<ActionResult<Member>> GetMemberById(string id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpPost("join")]
        public async Task<ActionResult<Member>> JoinMember(Member member)
        {
            var insertedMember = await _memberService.InsertMemberAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = insertedMember.Id }, insertedMember);
        }

        [HttpPost("myInfoEdit")]
        public async Task<IActionResult> EditMyInfo(Member member)
        {
            var success = await _memberService.UpdateMemberAsync(member);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Member>> Login(Member member)
        {
            // Implement login logic (authentication) here
            // For example, validate credentials against database

            var existingMember = await _memberService.GetMemberByIdAsync(member.Id);
            if (existingMember == null || existingMember.Pw != member.Pw)
            {
                return NotFound("Invalid credentials");
            }
            return Ok(existingMember);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Implement logout logic here
            // For example, invalidate session or token

            return NoContent();
        }
    }
}
```

**추가 사항**

1. 의존성 주입: Startup.cs에서 MemberService와 MemberDAO를 의존성 주입해야 합니다.
2. 예외 처리: 데이터베이스 연결 실패, 쿼리 오류 등에 대한 예외 처리가 필요합니다.
3. 보안: 비밀번호 저장은 안전한 방법(해싱 등)으로 처리해야 합니다.
4. 성능 최적화: 데이터베이스 쿼리의 성능을 위해 인덱스 설정 등을 고려해야 합니다.
5. 단위 테스트: 각 클래스와 메서드에 대한 단위 테스트 작성이 필요합니다.

<br><br><br>

# 11. Email 관련

## 11-1. Models/EmailRequest.cs

- 이메일 요청 데이터를 나타내는 모델 클래스를 작성합니다.

```csharp
namespace company.Models
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
```

<br><br>

## 11-2. Services/EmailService.cs

- 이메일 전송 로직을 처리하는 서비스를 작성합니다.

```csharp
using MailKit.Net.Smtp;
using MimeKit;
using company.Models;

namespace company.Services
{
    public class EmailService
    {
        public void SendEmail(EmailRequest emailRequest)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App", "your-email@example.com")); // 발신자 이메일 주소
            message.To.Add(new MailboxAddress(emailRequest.ToName, emailRequest.To));
            message.Subject = emailRequest.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = emailRequest.Body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.example.com", 587, false); // SMTP 서버 및 포트 설정
                client.Authenticate("your-email@example.com", "your-email-password"); // SMTP 인증 정보 설정
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
```

<br><br>

## 11-3. Controllers/EmailController.cs

- 이메일 전송 요청을 처리하는 컨트롤러를 작성합니다.

```csharp
using Microsoft.AspNetCore.Mvc;
using company.Services;
using company.Models;

namespace company.Controllers
{
    [ApiController]
    [Route("company/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.To) || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Body))
            {
                return BadRequest("Invalid email request.");
            }

            _emailService.SendEmail(emailRequest);
            return Ok("Email sent successfully.");
        }
    }
}
```
