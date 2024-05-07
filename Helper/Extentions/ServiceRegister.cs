using GlassECommerce.Services;
using GlassECommerce.Services.Interfaces;
namespace GlassECommerce.Helper.Extentions
{
    public static class ServiceRegister
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEnquiryService, EnquiryService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IWarehouseLogService, WarehouseLogService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IWishlistService, WishlistService>();
        }
    }
}
