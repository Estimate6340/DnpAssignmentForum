using RepositoryContracts;
using InMemoryRepositories;
using CLI.UI;

IUserRepository userRepo = new UserInMemoryRepository();
IPostRepository postRepo = new PostInMemoryRepository();
ICommentRepository commentRepo = new CommentInMemoryRepository();

var cli = new CliApp(userRepo, postRepo, commentRepo);
await cli.StartAsync();