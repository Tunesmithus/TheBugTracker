using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Models.ViewModels;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTLookUpService _lookUpService;
        private readonly IBTProjectService _projectService;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTRolesService _rolesService;
        private readonly IBTFileService _fileService;

        public ProjectsController(ApplicationDbContext context, 
                                  IBTLookUpService lookUpService, 
                                  IBTRolesService rolesService,
                                  IBTFileService fileService, 
                                  IBTProjectService projectService,
                                  IBTCompanyInfoService companyInfoService,
                                  UserManager<BTUser> userManager
                                  )
        {
            _context = context;
            _lookUpService = lookUpService;
            _rolesService = rolesService;
            _fileService = fileService;
            _projectService = projectService;
            _companyInfoService = companyInfoService;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projects.Include(p => p.Company).Include(p => p.ProjectPriority);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyProjects()
        {
            string userId = _userManager.GetUserId(User);
            List<Project> projects = await _projectService.GetUserProjectsAsync(userId);
            return View(projects);
        }

        public async Task<IActionResult> AllProjects()
        {
            int companyId = User.Identity.GetCompanyId().Value;

            List<Project> projects = new(); 

            if (User.IsInRole(Roles.Admin.ToString()) || User.IsInRole(Roles.ProjectManager.ToString()))
            {
                projects = await _companyInfoService.GetAllProjectsAsync(companyId);
            }
            else
            {
                projects = await _projectService.GetAllProjectsByCompany(companyId);
            }

            return View(projects);
            
        }


        public async Task<IActionResult> ArchivedProjects()
        {
            int companyId = User.Identity.GetCompanyId().Value;

            List<Project> archivedProjects = await _projectService.GetArchivedProjectsByCompany(companyId);

            return View(archivedProjects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int companyId = User.Identity.GetCompanyId().Value;

            Project project = await _projectService.GetProjectByIdAsync(id.Value, companyId);


            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            //Get company Id
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate view model
            AddProjectWithPMViewModel model = new();

            //Load select list with data
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(Roles.ProjectManager.ToString(), companyId), "Id", "FullName" );
            model.PriorityList = new SelectList(await _lookUpService.GetProjectPrioritiesAsync(),"Id", "Name");

            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProjectWithPMViewModel model)
        {
            if (model != null)
            {
                int companyId = User.Identity.GetCompanyId().Value;

                try
                {
                    if (model.Project.ImageFormFile != null)
                    {
                        model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                        model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                        model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                    }

                    model.Project.CompanyId = companyId;

                    await _projectService.AddNewProjectAsync(model.Project);

                    //Add PM if one was chosen
                    if (!string.IsNullOrEmpty(model.PmId))
                    {
                        await _projectService.AddProjectManagerAsync(model.PmId, model.Project.Id);
                    }

                }
                catch (Exception)
                {

                    throw;
                }

                //TODO:  Redirect to ALL Projects

                return RedirectToAction("Index");

            }
                
            
            return RedirectToAction(nameof(Create));
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate view model
            AddProjectWithPMViewModel model = new();

            model.Project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            //Load select list with data adn PMList & PriorityList
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(Roles.ProjectManager.ToString(), companyId), "Id", "FullName");
            model.PriorityList = new SelectList(await _lookUpService.GetProjectPrioritiesAsync(), "Id", "Name");

            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddProjectWithPMViewModel model)
        {
            
            if (model != null)
            {

                try
                {
                    if (model.Project.ImageFormFile != null)
                    {
                        model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                        model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                        model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                    }

                    await _projectService.UpdateProjectAsync(model.Project);

                    //Add PM if one was chosen
                    if (!string.IsNullOrEmpty(model.PmId))
                    {
                        await _projectService.AddProjectManagerAsync(model.PmId, model.Project.Id);
                    }

                }
                catch (Exception)
                {

                    throw;
                }

                //TODO:  Redirect to ALL Projects

                return RedirectToAction(nameof(Index));

            }

            return RedirectToAction(nameof(Create));

        }

        // GET: Projects/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);
               
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Archive/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            var project = await _projectService.GetProjectByIdAsync(id, companyId);

            await _projectService.ArchiveProjectAsync(project);

            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            var project = await _projectService.GetProjectByIdAsync(id, companyId);

            await _projectService.RestoreProjectAsync(project);

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
