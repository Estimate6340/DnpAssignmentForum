using RepositoryContracts;
using FileRepositories;
using CLI.UI;

IUserRepository userRepo = new UserFileRepository();
IPostRepository postRepo = new PostFileRepository();
ICommentRepository commentRepo = new CommentFileRepository();

var cli = new CliApp(userRepo, postRepo, commentRepo);
await cli.StartAsync();