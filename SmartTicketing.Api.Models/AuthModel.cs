namespace SmartTicketing.Api.Models
{
    // have them in a new project to be able to compose the requests easily in the integration tests
    // when the app will grow, maybe more models will be here that will be mapped to a DTO (test perfromance if AutoMapper)
    // for now a DTO is not needed in this phase and it should not be overengineed
    // keep it simple until the next feature set are defined in the vision and the architecture is in place
    // at this point the Entities will be used in services and controllers
    public class AuthModel
    {
        public string User { get; set; }

        public string Password { get; set; }
    }
}