# .NET 10.0 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
  - [OrchardCore.KtuSaTheme](#orchardcorektusatheme)
  - [OrchardCore.Cms.KtuSaModule](#orchardcorecmsktusamodule)
  - [KtuSaHeadlessCMS](#ktusaheadlesscms)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Risk Management](#risk-management)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Overview
Upgrade all projects in the KtuSaHeadlessCMS solution from .NET 8.0 to .NET 10.0 (Long Term Support).

### Scope
- **Projects**: 3 projects (1 ASP.NET Core application, 2 class libraries)
- **Current State**: All projects target net8.0
- **Target State**: All projects target net10.0
- **Total Codebase**: 4,882 lines of code across 171 files
- **Dependencies**: 19 NuGet packages (all compatible with .NET 10.0)

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in single operation.

**Rationale**: 
- Small solution (3 projects only)
- All currently on .NET 8.0
- Simple linear dependency structure (depth = 2)
- All 19 packages already compatible with .NET 10.0 (no package updates required)
- Low complexity across all projects
- No security vulnerabilities
- Total codebase under 5,000 LOC
- Good candidate for atomic upgrade

### Discovered Metrics
- **Dependency Depth**: 2 levels (simple)
- **Risk Indicators**: None (no security issues, no package updates required)
- **Complexity Classification**: Simple Solution
- **API Breaking Changes**: 2 behavioral changes (low impact)

### Critical Issues
- ✅ **No security vulnerabilities**
- ✅ **No incompatible packages**
- ⚠️ **2 behavioral API changes** requiring runtime testing validation

### Iteration Strategy
**Fast Batch Approach** (2-3 detail iterations):
- Iteration 1: Skeleton → Discovery → Strategy ✓
- Iteration 2: Foundation (Dependency Analysis, Migration Strategy, Project Stubs)
- Iteration 3: Details (All projects, Breaking Changes, Testing, Source Control, Success Criteria)

[To be filled]

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

All 3 projects will be upgraded to .NET 10.0 simultaneously in a single coordinated operation.

### Justification

This solution is an ideal candidate for All-At-Once approach:

**Size & Complexity**:
- Only 3 projects (well under 30-project threshold)
- Small codebase (4,882 LOC total)
- All projects rated Low difficulty in assessment
- Simple dependency chain with depth of 2

**Current State Uniformity**:
- All projects currently on .NET 8.0 (no .NET Framework migration needed)
- All projects using SDK-style format
- Homogeneous technology stack (OrchardCore CMS)

**Dependency Simplicity**:
- Clean linear dependency structure
- No circular dependencies
- No multi-targeting requirements
- Clear dependency order: Theme → Module → Application

**Package Compatibility**:
- All 19 packages already compatible with .NET 10.0
- No package version updates required
- No security vulnerabilities to address
- No breaking changes in external dependencies

**Low Risk Profile**:
- Only 2 behavioral API changes (both low impact)
- No binary or source incompatibilities
- Estimated <2 LOC modifications across solution
- All projects have similar complexity levels

### All-At-Once Strategy Rationale

The All-At-Once approach provides optimal value here:

**Benefits for This Solution**:
- **Fastest completion**: Single upgrade operation vs 3 sequential phases
- **No multi-targeting complexity**: Projects don't need to support multiple frameworks simultaneously
- **Clean dependency resolution**: All projects reference same framework at once
- **Simpler coordination**: No need to manage inter-project compatibility during transition
- **Immediate benefit**: All projects leverage .NET 10.0 improvements together

**Minimal Drawbacks**:
- Risk is manageable given low complexity and high compatibility
- Testing surface is small (3 projects, ~5k LOC)
- No need for staged rollout in development environment

### Execution Approach

**Single Atomic Operation**:
1. Update all 3 project files to target net10.0
2. Restore dependencies
3. Build entire solution
4. Fix any compilation errors from behavioral changes
5. Validate solution builds with 0 errors

**No Intermediate States**: Solution moves directly from "all net8.0" to "all net10.0" without partial migration states.

### Ordering Principles

While all projects upgrade simultaneously, awareness of dependency order is maintained:
- **OrchardCore.KtuSaTheme** (leaf): Changes here don't affect dependencies (none exist)
- **OrchardCore.Cms.KtuSaModule** (middle): Depends on Theme, consumed by Application
- **KtuSaHeadlessCMS** (root): Consumes both libraries

Any compilation errors will be addressed in dependency order if needed, though simultaneous update typically resolves compatibility automatically.

### Parallel vs Sequential Execution

**Within Atomic Operation**: Logical parallel execution
- All project file updates can occur simultaneously
- Build/restore operations follow naturally after all updates complete
- Error fixing addresses issues across all projects in single pass

### Validation Approach

**Single Comprehensive Validation**: After atomic upgrade completes
- Solution builds successfully
- All behavioral changes validated through testing
- No intermediate validation checkpoints needed

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a simple, linear dependency structure with no cycles:

```
KtuSaHeadlessCMS (ASP.NET Core App)
├── OrchardCore.Cms.KtuSaModule (Class Library)
│   └── OrchardCore.KtuSaTheme (Class Library)
└── OrchardCore.KtuSaTheme (Class Library)
```

**Dependency Flow**:
- **OrchardCore.KtuSaTheme**: Leaf node (0 project dependencies, 2 dependants)
- **OrchardCore.Cms.KtuSaModule**: Middle layer (1 dependency, 1 dependant)
- **KtuSaHeadlessCMS**: Root application (2 dependencies, 0 dependants)

### Project Groupings by Migration Phase

**All-At-Once Strategy**: All 3 projects upgraded simultaneously in single atomic operation.

**Logical Grouping** (for context, not sequential execution):
1. **Foundation Layer**: OrchardCore.KtuSaTheme (theme library)
2. **Module Layer**: OrchardCore.Cms.KtuSaModule (CMS module)
3. **Application Layer**: KtuSaHeadlessCMS (main ASP.NET Core application)

### Critical Path Identification

**Shortest dependency path**: OrchardCore.KtuSaTheme → OrchardCore.Cms.KtuSaModule → KtuSaHeadlessCMS (depth = 2)

**No blocking factors**:
- No circular dependencies
- All packages already compatible
- No conflicting framework requirements
- All projects using SDK-style format

### External Dependencies

All 19 NuGet packages are marked as **compatible** with .NET 10.0 in the assessment. No package updates required:
- OrchardCore packages (v1.8.3): 14 packages
- Google.Cloud.Storage.V1 (v4.10.0): 1 package
- HtmlAgilityPack (v1.11.71): 1 package
- Lombiq.LoginAsAnybody (v3.0.0): 1 package
- OrchardCoreContrib.Apis.Swagger (v1.4.1): 1 package

---

## Project-by-Project Migration Plans

### OrchardCore.KtuSaTheme

**Current State**: net8.0, ClassLibrary, SDK-style, 24 LOC, 0 dependencies, 2 dependants

**Target State**: net10.0

**Risk Level**: 🟢 Low

#### Migration Steps

**1. Prerequisites**
- None (leaf project, no project dependencies to upgrade first)

**2. Target Framework Update**
- Update `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>` in `OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj`

**3. Package Updates**
- No package updates required (all 5 packages already compatible)

**4. Expected Breaking Changes**
- **None**: Assessment shows 0 API issues for this project
- Theme structure and OrchardCore theme patterns remain consistent

**5. Code Modifications**
- **None expected**: Theme projects typically contain minimal code (views, assets, manifest)
- Standard OrchardCore theme structure should migrate cleanly

**6. Testing Strategy**
- **Build verification**: Project builds without errors or warnings
- **Theme rendering**: Validate theme still loads in application (tested when KtuSaHeadlessCMS runs)
- **Asset delivery**: Verify static assets (CSS, JS) serve correctly

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] Theme manifest valid
- [ ] Theme loads in application (integration test)

### OrchardCore.Cms.KtuSaModule

**Current State**: net8.0, ClassLibrary, SDK-style, 4,825 LOC, 1 dependency, 1 dependant

**Target State**: net10.0

**Risk Level**: 🟢 Low

#### Migration Steps

**1. Prerequisites**
- OrchardCore.KtuSaTheme upgraded to net10.0 (upgraded simultaneously in atomic operation)

**2. Target Framework Update**
- Update `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>` in `OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj`

**3. Package Updates**
- No package updates required (all 12 packages already compatible)

| Package | Current Version | Status | Notes |
|---------|----------------|--------|-------|
| OrchardCore.ContentFields | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.ContentManagement | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.ContentTypes.Abstractions | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.DisplayManagement | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Google | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Html | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Localization | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Module.Targets | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Navigation.Core | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.ResourceManagement | 1.8.3 | ✅ Compatible | No update needed |
| Google.Cloud.Storage.V1 | 4.10.0 | ✅ Compatible | No update needed |
| HtmlAgilityPack | 1.11.71 | ✅ Compatible | No update needed |

**4. Expected Breaking Changes**

**API Behavioral Change**:
- **`System.Net.Http.HttpContent`**: Behavioral change in .NET 10.0
  - **Impact**: If module uses HttpClient or makes HTTP requests, behavior may differ
  - **Action Required**: Review HTTP client usage, validate API calls work as expected
  - **Files to Review**: Search for `HttpClient`, `HttpContent`, `SendAsync` usage in module

**5. Code Modifications**

**Areas Requiring Review**:
- **HTTP Client Usage**: Locate any files using `System.Net.Http.HttpContent` or `HttpClient`
  - Validate serialization/deserialization behavior
  - Test API integrations (Google Cloud Storage, external APIs)
- **Module Services**: Review dependency injection and service registrations
- **Content Type Handlers**: Verify OrchardCore content handlers work correctly
- **Navigation Providers**: Validate navigation menu generation

**Configuration Updates**: None expected (OrchardCore modules use standard patterns)

**6. Testing Strategy**

**Unit Tests**: No test project identified in solution
- **Recommendation**: Create integration tests for critical module functionality

**Integration Testing**:
- Module loads successfully in OrchardCore
- Content types register correctly
- HTTP operations (if any) function as expected
- Google Cloud Storage integration works (if module uses it)
- Navigation and admin UI render correctly

**Manual Testing**:
- Verify module appears in OrchardCore admin
- Test content type operations
- Validate any custom API endpoints

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] Module manifest valid
- [ ] Module loads in OrchardCore application
- [ ] HTTP operations tested (if applicable)
- [ ] Content type operations functional

### KtuSaHeadlessCMS

**Current State**: net8.0, AspNetCore, SDK-style, 33 LOC, 2 dependencies, 0 dependants

**Target State**: net10.0

**Risk Level**: 🟢 Low

#### Migration Steps

**1. Prerequisites**
- OrchardCore.KtuSaTheme upgraded to net10.0 (upgraded simultaneously in atomic operation)
- OrchardCore.Cms.KtuSaModule upgraded to net10.0 (upgraded simultaneously in atomic operation)

**2. Target Framework Update**
- Update `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>` in `KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj`

**3. Package Updates**
- No package updates required (all 4 packages already compatible)

| Package | Current Version | Status | Notes |
|---------|----------------|--------|-------|
| OrchardCore.Application.Cms.Targets | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Google | 1.8.3 | ✅ Compatible | No update needed |
| OrchardCore.Logging.NLog | 1.8.3 | ✅ Compatible | No update needed |
| Lombiq.LoginAsAnybody | 3.0.0 | ✅ Compatible | No update needed |
| OrchardCoreContrib.Apis.Swagger | 1.4.1 | ✅ Compatible | No update needed |

**4. Expected Breaking Changes**

**API Behavioral Change**:
- **`Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler(IApplicationBuilder, String)`**: Behavioral change in .NET 10.0
  - **Impact**: Exception handling middleware behavior may differ
  - **Location**: `Program.cs` line 22 - `app.UseExceptionHandler("/Error");`
  - **Action Required**: Validate exception handling works correctly, verify error page routing

**5. Code Modifications**

**File: `Program.cs`**
- **Review `UseExceptionHandler` call**: 
  - Current usage: `app.UseExceptionHandler("/Error");`
  - Verify error page route still valid
  - Test exception handling in non-development mode
  - Consult .NET 10.0 ASP.NET Core migration guide for changes

**Configuration Review**:
- **`appsettings.json`**: Verify no deprecated configuration keys
- **NLog configuration**: Confirm logging works with .NET 10.0
- **OrchardCore settings**: Validate CMS configuration patterns

**Areas Requiring Review**:
- Exception handling middleware configuration
- HTTPS redirection (line 25: `app.UseHttpsRedirection();`)
- Static files middleware (line 26: `app.UseStaticFiles();`)
- OrchardCore middleware (line 28: `app.UseOrchardCore();`)
- Settings binding pattern for `GoogleCloudSettings` and `FientaSettings`

**6. Testing Strategy**

**Build Verification**:
- Application builds without errors
- Application builds without warnings

**Runtime Testing**:
- Application starts successfully
- Exception handling works (trigger error, verify "/Error" route)
- HTTPS redirection functions correctly
- Static files serve properly
- OrchardCore CMS loads and initializes
- Google Cloud settings bind correctly
- Fienta settings bind correctly
- Admin login works (Lombiq.LoginAsAnybody plugin)
- Swagger UI accessible (if enabled)

**Environment Testing**:
- Development mode: Exception page shows details
- Production mode: Generic error page via UseExceptionHandler

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] Application starts successfully
- [ ] Exception handling verified
- [ ] OrchardCore CMS initializes
- [ ] Configuration settings loaded correctly
- [ ] Static assets served
- [ ] All middleware functional

---

## Package Update Reference

### Summary

**Total Packages**: 19  
**Packages Requiring Updates**: 0  
**All packages are already compatible with .NET 10.0**

### Package Compatibility Matrix

| Package | Current Version | Target Version | Projects Affected | Update Reason |
|---------|----------------|----------------|-------------------|---------------|
| Google.Cloud.Storage.V1 | 4.10.0 | No change | 1 project | Already compatible |
| HtmlAgilityPack | 1.11.71 | No change | 1 project | Already compatible |
| Lombiq.LoginAsAnybody | 3.0.0 | No change | 1 project | Already compatible |
| OrchardCore.Admin | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.Application.Cms.Targets | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.ContentFields | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.ContentManagement | 1.8.3 | No change | 2 projects | Already compatible |
| OrchardCore.Contents | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.ContentTypes.Abstractions | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.DisplayManagement | 1.8.3 | No change | 2 projects | Already compatible |
| OrchardCore.Google | 1.8.3 | No change | 2 projects | Already compatible |
| OrchardCore.Html | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.Localization | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.Logging.NLog | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.Module.Targets | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.Navigation.Core | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCore.ResourceManagement | 1.8.3 | No change | 2 projects | Already compatible |
| OrchardCore.Theme.Targets | 1.8.3 | No change | 1 project | Already compatible |
| OrchardCoreContrib.Apis.Swagger | 1.4.1 | No change | 1 project | Already compatible |

### Package Groups

**OrchardCore Core Packages** (14 packages, v1.8.3):
- All marked compatible by assessment
- OrchardCore 1.8.3 released with .NET 8.0 support
- ⚠️ **Validation needed**: Confirm OrchardCore 1.8.3 officially supports .NET 10.0
- If incompatibility found: Consider upgrading to OrchardCore 1.9+ (if available)

**Third-Party Packages** (5 packages):
- Google.Cloud.Storage.V1 4.10.0 (Google Cloud client library)
- HtmlAgilityPack 1.11.71 (HTML parsing)
- Lombiq.LoginAsAnybody 3.0.0 (development plugin)
- OrchardCoreContrib.Apis.Swagger 1.4.1 (API documentation)
- All confirmed compatible by assessment

### Optional Updates

While no updates are required for .NET 10.0 compatibility, consider updating to latest stable versions for:
- **Security patches**: Check if newer versions address CVEs
- **Performance improvements**: Newer versions may have optimizations
- **New features**: Take advantage of latest capabilities

**Defer to post-upgrade**: Package updates can be performed after successful .NET 10.0 migration to isolate framework upgrade issues from package update issues.

---

## Breaking Changes Catalog

### .NET 8.0 → .NET 10.0 Behavioral Changes

Based on assessment findings, the following APIs have behavioral changes:

#### 1. `Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler`

**Location**: `KtuSaHeadlessCMS\Program.cs:22`

**Current Usage**:
```csharp
app.UseExceptionHandler("/Error");
```

**Behavioral Change**: Exception handling middleware behavior modified in .NET 10.0

**Impact**: Medium
- Exception handling flow may differ
- Error page routing may behave differently
- Exception details exposure may change

**Migration Path**:
1. Review .NET 10.0 ASP.NET Core exception handling documentation
2. Verify "/Error" route exists and is accessible
3. Test exception handling in both Development and Production modes
4. Validate error details are appropriately hidden in Production
5. Consider reviewing `StatusCodePagesMiddleware` integration if used

**Testing Requirements**:
- Trigger unhandled exception, verify error page displays
- Check exception details in Development mode
- Verify generic error in Production mode
- Test with various exception types

---

#### 2. `System.Net.Http.HttpContent`

**Location**: `OrchardCore.Cms.KtuSaModule` (specific files not identified, requires code search)

**Behavioral Change**: HttpContent behavior modified in .NET 10.0

**Impact**: Medium
- HTTP request/response handling may differ
- Serialization behavior may change
- Content reading patterns may behave differently

**Migration Path**:
1. Identify all HttpContent usage in module:
   - Search for `HttpClient`, `HttpContent`, `SendAsync`, `ReadAsStringAsync`, `ReadFromJsonAsync`
2. Review .NET 10.0 HTTP client breaking changes documentation
3. Validate HTTP operations:
   - Google Cloud Storage API calls
   - Any external API integrations
   - Content serialization/deserialization
4. Test edge cases (large payloads, streaming, cancellation)

**Testing Requirements**:
- Test all external API integrations
- Verify Google Cloud Storage operations
- Validate JSON serialization/deserialization
- Test error handling in HTTP operations

---

### OrchardCore 1.8.3 Compatibility

**Potential Issue**: OrchardCore 1.8.3 released before .NET 10.0

**Validation Required**:
- Check OrchardCore release notes for .NET 10.0 support
- Test OrchardCore initialization and core functionality
- Monitor OrchardCore GitHub for compatibility reports

**If Incompatible**:
- Upgrade to OrchardCore 1.9+ (if available with .NET 10.0 support)
- Or target .NET 9.0 (STS) until OrchardCore catches up
- Contact OrchardCore community for guidance

### Implementation Timeline

This timeline reflects the All-At-Once Strategy with a single atomic upgrade phase.

#### Phase 0: Preparation (Prerequisites)

**Verification Steps**:
- Confirm .NET 10.0 SDK installed on development machine
- Validate current branch is `upgrade-to-NET10`
- Ensure clean working directory (no uncommitted changes from assessment)

**Deliverables**: Environment ready for upgrade

---

#### Phase 1: Atomic Upgrade

**Operations** (performed as single coordinated batch):

1. **Update all project files to net10.0**:
   - OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj
   - OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj
   - KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj

2. **Restore dependencies**: `dotnet restore`

3. **Build solution and fix all compilation errors**:
   - Execute `dotnet build` on entire solution
   - Address any compilation errors discovered
   - Review behavioral change locations (Program.cs, HttpContent usage)
   - Apply fixes based on Breaking Changes Catalog

4. **Verify build success**: Solution builds with 0 errors, 0 warnings

**Deliverables**: 
- All projects targeting net10.0
- Solution builds successfully
- No compilation errors or warnings

---

#### Phase 2: Validation & Testing

**Operations**:

1. **Runtime Validation**:
   - Start KtuSaHeadlessCMS application
   - Verify OrchardCore initializes
   - Test exception handling (UseExceptionHandler behavioral change)
   - Validate HTTP operations (HttpContent behavioral change)

2. **Integration Testing**:
   - Execute smoke tests (see Testing & Validation Strategy)
   - Test critical user flows
   - Verify OrchardCore admin functionality
   - Validate theme rendering
   - Test module features

3. **Address Test Failures** (if any):
   - Debug and fix issues discovered
   - Rebuild and re-test
   - Document any unexpected behaviors

**Deliverables**: 
- All tests pass
- Application functional
- Behavioral changes validated
- No runtime errors

---

#### Phase 3: Finalization

**Operations**:
- Commit all changes to `upgrade-to-NET10` branch
- Update documentation (README, setup guides)
- Create Pull Request to `master`
- Code review and approval
- Merge to `master`

**Deliverables**: 
- Upgrade complete and merged
- Documentation current
- Team notified of .NET 10.0 requirement

---

### Estimated Complexity

**Overall**: 🟢 Low

This is a straightforward upgrade with minimal risk and clear execution path.

---

### General .NET 10.0 Changes

**ASP.NET Core Middleware**:
- Review middleware ordering best practices
- Verify `UseHttpsRedirection` compatibility
- Confirm `UseStaticFiles` behavior unchanged
- Validate `UseHsts` configuration

**Configuration Binding**:
- Pattern used in `Program.cs` (lines 8-15) should remain compatible
- Verify `builder.Configuration.GetSection().Bind()` works as expected

**Dependency Injection**:
- Service registration patterns should be compatible
- Singleton service additions (lines 16-17) should work unchanged

**Hosting Model**:
- `UseNLogHost()` compatibility with .NET 10.0
- Verify NLog integration works correctly

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

#### Level 1: Build Verification (Immediate)

**After atomic upgrade completes**:
- [ ] All 3 projects build without errors
- [ ] All 3 projects build without warnings
- [ ] No package restore errors
- [ ] No dependency conflicts

**Success Criteria**: Solution builds with 0 errors, 0 warnings

---

#### Level 2: Project-Level Validation

**OrchardCore.KtuSaTheme**:
- [ ] Theme manifest loads correctly
- [ ] No runtime initialization errors
- [ ] Theme assets accessible

**OrchardCore.Cms.KtuSaModule**:
- [ ] Module manifest valid
- [ ] Module loads in OrchardCore
- [ ] HTTP operations functional (if applicable)
- [ ] Content type handlers work
- [ ] Navigation providers functional

**KtuSaHeadlessCMS**:
- [ ] Application starts without errors
- [ ] Exception handling works (`/Error` route)
- [ ] Configuration binding successful (GoogleCloudSettings, FientaSettings)
- [ ] OrchardCore CMS initializes
- [ ] NLog logging operational

---

#### Level 3: Integration Testing

**Middleware Stack**:
- [ ] `UseExceptionHandler("/Error")` - Behavioral change validated
- [ ] `UseHsts()` - HSTS headers present in Production
- [ ] `UseHttpsRedirection()` - HTTPS redirects functional
- [ ] `UseStaticFiles()` - Static content served
- [ ] `UseOrchardCore()` - CMS framework initializes

**OrchardCore Integration**:
- [ ] Admin dashboard accessible
- [ ] Content types registered correctly
- [ ] Theme renders in frontend
- [ ] Module features enabled
- [ ] Navigation menus display
- [ ] Google Cloud integration functional (if used)

**API Testing** (if Swagger enabled):
- [ ] Swagger UI accessible
- [ ] API endpoints respond correctly
- [ ] Authentication/authorization functional

---

#### Level 4: End-to-End Scenarios

**Critical User Flows**:
1. **Admin Access**: Login → Navigate admin → Perform content operations
2. **Content Management**: Create/edit/publish content using module features
3. **Frontend Rendering**: View public-facing pages with theme applied
4. **Error Handling**: Trigger error → Verify proper error page display
5. **API Operations**: Test any REST endpoints exposed by solution

**Performance Validation**:
- Application startup time acceptable
- Page load times within normal range
- No significant performance regressions

---

### Testing Execution Order

**Phase 1: Atomic Upgrade Testing**
1. Build verification (automated)
2. Application startup test (automated/manual)
3. Exception handling test (manual)

**Phase 2: Functional Testing**
4. OrchardCore initialization (manual)
5. Admin dashboard access (manual)
6. Theme rendering validation (manual)
7. Module feature verification (manual)

**Phase 3: Comprehensive Validation**
8. End-to-end scenario testing (manual)
9. Performance validation (manual)
10. Environment-specific testing (Development vs Production)

---

### Test Environment Setup

**Requirements**:
- .NET 10.0 SDK installed
- Development environment (local IIS Express or Kestrel)
- OrchardCore database accessible
- Google Cloud credentials available (if module uses them)
- Fienta API credentials available (if used)

**Test Data**:
- Existing OrchardCore content
- Sample content types from module
- Test user accounts

---

### Smoke Test Checklist

Quick validation after upgrade (5-10 minutes):
- [ ] `dotnet build` succeeds
- [ ] `dotnet run --project KtuSaHeadlessCMS` starts application
- [ ] Navigate to `https://localhost:xxxxx/` - homepage loads
- [ ] Navigate to `/Admin` - admin dashboard loads
- [ ] Trigger 404 error - error handling works
- [ ] No console errors in browser DevTools

**If smoke tests pass**: Proceed to comprehensive testing  
**If smoke tests fail**: Address blocking issues before full testing

---

## Detailed Execution Steps

This section provides the precise sequence of operations for the atomic upgrade.

### Step 1: Verify Prerequisites

**SDK Installation**:
- Confirm .NET 10.0 SDK installed: `dotnet --list-sdks`
- Expected: SDK version 10.0.x listed
- If missing: Download from https://dotnet.microsoft.com/download/dotnet/10.0

**Branch Confirmation**:
- Verify current branch: `git branch --show-current`
- Expected: `upgrade-to-NET10`
- Working directory clean (no uncommitted changes)

---

### Step 2: Update Project Files

Update `<TargetFramework>` element in all 3 project files simultaneously:

**File: `OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj`**
```xml
<TargetFramework>net8.0</TargetFramework>
→
<TargetFramework>net10.0</TargetFramework>
```

**File: `OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj`**
```xml
<TargetFramework>net8.0</TargetFramework>
→
<TargetFramework>net10.0</TargetFramework>
```

**File: `KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj`**
```xml
<TargetFramework>net8.0</TargetFramework>
→
<TargetFramework>net10.0</TargetFramework>
```

**No package updates required** - all packages already compatible per assessment.

---

### Step 3: Restore and Build

**Restore Dependencies**:
```bash
dotnet restore KtuSaHeadlessCMS.sln
```
- Expected outcome: All packages restore successfully
- No version conflicts expected (all packages compatible)

**Build Solution**:
```bash
dotnet build KtuSaHeadlessCMS.sln --no-restore
```
- Expected outcome: Solution builds with 0 errors (or minimal errors from behavioral changes)

---

### Step 4: Address Compilation Errors

**If Errors Occur**, consult Breaking Changes Catalog and address:

**UseExceptionHandler Behavioral Change** (if compilation error):
- Location: `KtuSaHeadlessCMS\Program.cs:22`
- Review .NET 10.0 migration guide for exception handling changes
- Update middleware configuration if required

**HttpContent Behavioral Change** (if compilation error):
- Location: `OrchardCore.Cms.KtuSaModule` (search for HttpClient/HttpContent usage)
- Review HTTP client usage patterns
- Update content reading/writing patterns if required

**Rebuild After Fixes**:
```bash
dotnet build KtuSaHeadlessCMS.sln --no-restore
```
- Target: 0 errors, 0 warnings

---

### Step 5: Runtime Validation

**Start Application**:
```bash
dotnet run --project KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj
```

**Validate Startup**:
- [ ] Application starts without exceptions
- [ ] NLog logging initializes
- [ ] OrchardCore CMS initializes
- [ ] Configuration binds correctly (GoogleCloudSettings, FientaSettings)
- [ ] No console errors

**Test Behavioral Changes**:
1. **Exception Handling**: 
   - Navigate to non-existent route to trigger 404
   - Trigger server error (if test endpoint available)
   - Verify error handling routes to "/Error" correctly

2. **HTTP Operations** (if module uses HttpClient):
   - Test any external API calls
   - Verify Google Cloud Storage operations (if used)
   - Confirm serialization/deserialization works

**OrchardCore Integration**:
- [ ] Navigate to `/Admin` - admin dashboard loads
- [ ] Theme renders correctly on frontend
- [ ] Module features accessible
- [ ] Content operations functional

---

### Step 6: Execute Smoke Tests

Run through smoke test checklist (see Testing & Validation Strategy):
- Build verification ✓ (completed in Step 4)
- Application startup ✓ (completed in Step 5)
- Homepage loads
- Admin dashboard accessible
- No browser console errors

Expected outcome: All smoke tests pass

---

### Step 7: Comprehensive Testing

Execute full testing strategy (see Testing & Validation Strategy § Level 4):
- End-to-end user scenarios
- Performance validation
- Environment-specific testing (Development/Production modes)

Expected outcome: All tests pass, no regressions

---

### Step 8: Finalize and Commit

**Commit Changes**:
```bash
git add .
git commit -m "chore: Upgrade solution to .NET 10.0 LTS

- Update all 3 projects from net8.0 to net10.0
- OrchardCore.KtuSaTheme: Framework update
- OrchardCore.Cms.KtuSaModule: Framework update + HttpContent behavioral validation
- KtuSaHeadlessCMS: Framework update + UseExceptionHandler behavioral validation
- All packages remain compatible at current versions
- Validated build, runtime, and OrchardCore integration

Breaking Changes:
- UseExceptionHandler behavioral change (tested)
- HttpContent behavioral change (tested)"
```

**Create Pull Request**:
- Title: "Upgrade solution to .NET 10.0 LTS"
- Description: Reference plan.md, assessment.md, testing results
- Assign reviewers

**Merge After Approval**: Merge `upgrade-to-NET10` → `master`

---

### Execution Summary

**Total Steps**: 8  
**Expected Complexity**: 🟢 Low  
**Atomic Operation**: Steps 2-4 form the core atomic upgrade (project updates → build → fix → verify)

---

## Risk Management

### Risk Profile

**Overall Risk Level**: 🟢 **Low**

This upgrade presents minimal risk due to:
- Small solution size (3 projects, <5k LOC)
- All packages already compatible with .NET 10.0
- No security vulnerabilities present
- Only 2 low-impact behavioral changes
- All projects currently on modern .NET 8.0
- Simple dependency structure with no cycles

### High-Risk Changes

**None identified**. All projects rated Low difficulty in assessment.

### Medium-Risk Considerations

| Area | Risk Description | Mitigation |
|------|------------------|------------|
| Behavioral API Changes | 2 APIs have behavioral changes in .NET 10:<br/>- `UseExceptionHandler` (KtuSaHeadlessCMS)<br/>- `HttpContent` (OrchardCore.Cms.KtuSaModule) | Validate exception handling behavior and HTTP content processing during runtime testing |
| OrchardCore Compatibility | OrchardCore 1.8.3 compatibility with .NET 10.0 needs validation | Build and runtime testing will confirm compatibility; OrchardCore typically supports new .NET versions |
| Configuration Changes | ASP.NET Core configuration patterns may have changed | Review `Program.cs` for deprecated patterns; consult .NET 10.0 migration guide |

### Contingency Plans

**If OrchardCore 1.8.3 incompatible with .NET 10.0**:
- Check for OrchardCore 1.9+ release supporting .NET 10.0
- Review OrchardCore release notes for .NET 10.0 support timeline
- Consider upgrading OrchardCore packages simultaneously
- Alternative: Target .NET 9.0 (STS) until OrchardCore .NET 10.0 support confirmed

**If behavioral changes cause runtime issues**:
- `UseExceptionHandler`: Review exception handling middleware configuration, consult migration documentation
- `HttpContent`: Review HTTP client usage in module, test API integrations thoroughly

**If compilation errors exceed expectations**:
- Consult .NET 10.0 breaking changes documentation
- Review OrchardCore migration guides
- Address errors systematically by project (Theme → Module → Application)

### Rollback Strategy

**Pre-Upgrade State**: Branch `master` preserved
**Upgrade Branch**: `upgrade-to-NET10`

**If Critical Issues Discovered**:
1. Document specific blocking issue
2. Switch back to `master` branch
3. Assess whether issue is addressable
4. Plan mitigation and retry, or defer upgrade

**Rollback is Clean**: No changes to master until upgrade validated and merged

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Risk | Rationale |
|---------|------------|--------------|------|-----------|
| OrchardCore.KtuSaTheme | 🟢 Low | 0 projects, 5 packages | Low | Small theme library (24 LOC), minimal code, standard OrchardCore theme structure |
| OrchardCore.Cms.KtuSaModule | 🟢 Low | 1 project, 12 packages | Low | Module library (4,825 LOC), 1 behavioral API change, standard module patterns |
| KtuSaHeadlessCMS | 🟢 Low | 2 projects, 4 packages | Low | Main application (33 LOC), minimal custom code, 1 behavioral API change in `Program.cs` |

### Phase Complexity Assessment

**Single Atomic Phase**: All-At-Once upgrade

**Complexity Rating**: 🟢 **Low**

**Factors**:
- All projects have aligned framework versions (net8.0)
- All packages compatible (no version conflicts to resolve)
- Minimal custom code (most LOC is OrchardCore infrastructure)
- Only 2 behavioral changes (well-documented .NET changes)
- Standard ASP.NET Core + OrchardCore patterns

### Dependency Ordering Awareness

While executing simultaneously, logical dependency order:
1. **OrchardCore.KtuSaTheme** - Foundation, consumed by both other projects
2. **OrchardCore.Cms.KtuSaModule** - Builds on Theme, consumed by Application
3. **KtuSaHeadlessCMS** - Top-level application consuming both

This ordering informs troubleshooting if issues arise (investigate leaf dependencies first).

### Resource Requirements

**Technical Skills Required**:
- .NET 8 → .NET 10 migration knowledge
- ASP.NET Core fundamentals
- OrchardCore CMS familiarity (helpful but not critical)
- NuGet package management

**Team Capacity**: Single developer sufficient

**Parallel Execution**: Not applicable (3-project solution, atomic operation)

---

## Source Control Strategy

### Branching Strategy

**Main Branch**: `master` (preserved at .NET 8.0 state)  
**Upgrade Branch**: `upgrade-to-NET10` (all upgrade work occurs here)

**Merge Approach**: Feature branch workflow
1. All upgrade work committed to `upgrade-to-NET10`
2. After validation complete, create Pull Request: `upgrade-to-NET10` → `master`
3. Code review before merge
4. Merge to `master` once approved

---

### Commit Strategy

**All-At-Once Single Commit Approach** (Recommended)

Given the atomic nature of this upgrade, use a single comprehensive commit:

**Commit Message Format**:
```
chore: Upgrade solution to .NET 10.0 LTS

- Update all 3 projects from net8.0 to net10.0
- OrchardCore.KtuSaTheme: Framework update
- OrchardCore.Cms.KtuSaModule: Framework update + HttpContent behavioral validation
- KtuSaHeadlessCMS: Framework update + UseExceptionHandler behavioral validation
- All packages remain compatible at current versions
- Validated build, runtime, and OrchardCore integration

Breaking Changes:
- UseExceptionHandler behavioral change (tested)
- HttpContent behavioral change (tested)

Refs: #[issue-number] (if tracking in issue system)
```

**Rationale**:
- Framework changes across 3 projects are interdependent
- Single atomic commit reflects All-At-Once strategy
- Easier to review as unified change
- Simpler to revert if needed (single commit vs multiple)
- Clear history: "This commit upgraded to .NET 10.0"

---

**Alternative: Checkpoint Commits** (If preferred)

If multiple commits desired for granular tracking:

```
Commit 1: Update all project files to net10.0
Commit 2: Fix compilation errors (if any)
Commit 3: Validate and document behavioral changes
```

Each commit message follows format:
```
chore(upgrade): <specific change>

Part of .NET 10.0 upgrade
[detailed description]
```

---

### Review and Merge Process

**Pull Request Requirements**:
- **Title**: "Upgrade solution to .NET 10.0 LTS"
- **Description**:
  - Link to `plan.md` and `assessment.md`
  - Summary of changes (3 projects upgraded)
  - Testing performed (build, runtime, integration)
  - Known behavioral changes addressed
  - Validation results
- **Reviewers**: Assign team lead or senior developer

**PR Checklist**:
- [ ] All projects build successfully
- [ ] Application runs without errors
- [ ] OrchardCore CMS initializes correctly
- [ ] Exception handling validated
- [ ] HTTP operations tested (if applicable)
- [ ] No performance regressions
- [ ] Documentation updated (if README mentions .NET version)

**Merge Criteria**:
- All checklist items completed
- At least one approval from reviewer
- No blocking comments unresolved
- CI/CD pipeline passes (if configured)

---

### Rollback Plan

**If Issues Discovered Post-Merge**:
1. Revert merge commit on `master`
2. Return to `upgrade-to-NET10` branch
3. Address issues identified
4. Re-test and create new PR

**Branch Preservation**:
- Keep `upgrade-to-NET10` branch until changes validated in production
- Delete branch only after confirming upgrade successful

---

## Success Criteria

### Technical Criteria

**All projects migrated**:
- [ ] OrchardCore.KtuSaTheme targets net10.0
- [ ] OrchardCore.Cms.KtuSaModule targets net10.0
- [ ] KtuSaHeadlessCMS targets net10.0

**All packages compatible**:
- [ ] No package dependency conflicts
- [ ] All 19 packages restore successfully
- [ ] No security vulnerabilities remain (none in current state)

**Builds pass**:
- [ ] Solution builds with 0 errors
- [ ] Solution builds with 0 warnings
- [ ] All projects compile successfully

**Runtime validation**:
- [ ] KtuSaHeadlessCMS application starts successfully
- [ ] OrchardCore CMS initializes without errors
- [ ] Exception handling works correctly (behavioral change validated)
- [ ] HTTP operations functional (behavioral change validated)

---

### Quality Criteria

**Code quality maintained**:
- [ ] No introduction of code smells
- [ ] Configuration patterns remain clean
- [ ] Dependency injection properly configured
- [ ] Error handling comprehensive

**Documentation updated**:
- [ ] README updated with .NET 10.0 requirement (if applicable)
- [ ] Development setup instructions reflect .NET 10.0 SDK
- [ ] Configuration documentation current

**Test coverage maintained**:
- [ ] All manual integration tests performed
- [ ] Critical user flows validated
- [ ] Regression testing complete

---

### Process Criteria

**All-At-Once Strategy followed**:
- [ ] All 3 projects upgraded simultaneously
- [ ] Single atomic operation (no intermediate states)
- [ ] Unified testing approach
- [ ] Comprehensive validation before merge

**Source control strategy followed**:
- [ ] All work on `upgrade-to-NET10` branch
- [ ] Commit strategy executed (single commit or checkpoint commits as chosen)
- [ ] Pull Request created with complete description
- [ ] Code review completed
- [ ] PR merged to `master` after approval

**Risk management applied**:
- [ ] Behavioral changes identified and tested
- [ ] OrchardCore compatibility validated
- [ ] Rollback plan available if needed

---

### Definition of Done

The .NET 10.0 upgrade is **complete** when:

1. ✅ All technical criteria met (builds, runs, no errors)
2. ✅ All quality criteria met (code quality, documentation updated)
3. ✅ All process criteria met (strategy followed, PR approved)
4. ✅ Changes merged to `master` branch
5. ✅ Application validated in target deployment environment (if applicable)

**Final Deliverables**:
- `plan.md` - This planning document
- `assessment.md` - Assessment report
- `tasks.md` - Task execution tracking (generated during execution stage)
- Updated solution on `master` branch targeting .NET 10.0
