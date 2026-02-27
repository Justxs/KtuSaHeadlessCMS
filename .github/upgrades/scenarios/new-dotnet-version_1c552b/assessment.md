# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj)
  - [OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj)
  - [OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 3 | All require upgrade |
| Total NuGet Packages | 19 | All compatible |
| Total Code Files | 137 |  |
| Total Code Files with Incidents | 5 |  |
| Total Lines of Code | 4882 |  |
| Total Number of Issues | 5 |  |
| Estimated LOC to modify | 2+ | at least 0,0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj) | net8.0 | 🟢 Low | 0 | 1 | 1+ | AspNetCore, Sdk Style = True |
| [OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | net8.0 | 🟢 Low | 0 | 1 | 1+ | ClassLibrary, Sdk Style = True |
| [OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | net8.0 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 19 | 100,0% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 0 | 0,0% |
| ***Total NuGet Packages*** | ***19*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 2 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 17001 |  |
| ***Total APIs Analyzed*** | ***17003*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Google.Cloud.Storage.V1 | 4.10.0 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| HtmlAgilityPack | 1.11.71 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| Lombiq.LoginAsAnybody | 3.0.0 |  | [KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj) | ✅Compatible |
| OrchardCore.Admin | 1.8.3 |  | [OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCore.Application.Cms.Targets | 1.8.3 |  | [KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj) | ✅Compatible |
| OrchardCore.ContentFields | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.ContentManagement | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj)<br/>[OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCore.Contents | 1.8.3 |  | [OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCore.ContentTypes.Abstractions | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.DisplayManagement | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj)<br/>[OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCore.Google | 1.8.3 |  | [KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj)<br/>[OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.Html | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.Localization | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.Logging.NLog | 1.8.3 |  | [KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj) | ✅Compatible |
| OrchardCore.Module.Targets | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.Navigation.Core | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj) | ✅Compatible |
| OrchardCore.ResourceManagement | 1.8.3 |  | [OrchardCore.Cms.KtuSaModule.csproj](#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj)<br/>[OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCore.Theme.Targets | 1.8.3 |  | [OrchardCore.KtuSaTheme.csproj](#orchardcorektusathemeorchardcorektusathemecsproj) | ✅Compatible |
| OrchardCoreContrib.Apis.Swagger | 1.4.1 |  | [KtuSaHeadlessCMS.csproj](#ktusaheadlesscmsktusaheadlesscmscsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| M:Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler(Microsoft.AspNetCore.Builder.IApplicationBuilder,System.String) | 1 | 50,0% | Behavioral Change |
| T:System.Net.Http.HttpContent | 1 | 50,0% | Behavioral Change |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;KtuSaHeadlessCMS.csproj</b><br/><small>net8.0</small>"]
    P2["<b>📦&nbsp;OrchardCore.Cms.KtuSaModule.csproj</b><br/><small>net8.0</small>"]
    P3["<b>📦&nbsp;OrchardCore.KtuSaTheme.csproj</b><br/><small>net8.0</small>"]
    P1 --> P3
    P1 --> P2
    P2 --> P3
    click P1 "#ktusaheadlesscmsktusaheadlesscmscsproj"
    click P2 "#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj"
    click P3 "#orchardcorektusathemeorchardcorektusathemecsproj"

```

## Project Details

<a id="ktusaheadlesscmsktusaheadlesscmscsproj"></a>
### KtuSaHeadlessCMS\KtuSaHeadlessCMS.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 2
- **Dependants**: 0
- **Number of Files**: 7
- **Number of Files with Incidents**: 2
- **Lines of Code**: 33
- **Estimated LOC to modify**: 1+ (at least 3,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["KtuSaHeadlessCMS.csproj"]
        MAIN["<b>📦&nbsp;KtuSaHeadlessCMS.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#ktusaheadlesscmsktusaheadlesscmscsproj"
    end
    subgraph downstream["Dependencies (2"]
        P3["<b>📦&nbsp;OrchardCore.KtuSaTheme.csproj</b><br/><small>net8.0</small>"]
        P2["<b>📦&nbsp;OrchardCore.Cms.KtuSaModule.csproj</b><br/><small>net8.0</small>"]
        click P3 "#orchardcorektusathemeorchardcorektusathemecsproj"
        click P2 "#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj"
    end
    MAIN --> P3
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 63 |  |
| ***Total APIs Analyzed*** | ***64*** |  |

<a id="orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj"></a>
### OrchardCore.Cms.KtuSaModule\OrchardCore.Cms.KtuSaModule.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 155
- **Number of Files with Incidents**: 2
- **Lines of Code**: 4825
- **Estimated LOC to modify**: 1+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;KtuSaHeadlessCMS.csproj</b><br/><small>net8.0</small>"]
        click P1 "#ktusaheadlesscmsktusaheadlesscmscsproj"
    end
    subgraph current["OrchardCore.Cms.KtuSaModule.csproj"]
        MAIN["<b>📦&nbsp;OrchardCore.Cms.KtuSaModule.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj"
    end
    subgraph downstream["Dependencies (1"]
        P3["<b>📦&nbsp;OrchardCore.KtuSaTheme.csproj</b><br/><small>net8.0</small>"]
        click P3 "#orchardcorektusathemeorchardcorektusathemecsproj"
    end
    P1 --> MAIN
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 16125 |  |
| ***Total APIs Analyzed*** | ***16126*** |  |

<a id="orchardcorektusathemeorchardcorektusathemecsproj"></a>
### OrchardCore.KtuSaTheme\OrchardCore.KtuSaTheme.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 2
- **Number of Files**: 9
- **Number of Files with Incidents**: 1
- **Lines of Code**: 24
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;KtuSaHeadlessCMS.csproj</b><br/><small>net8.0</small>"]
        P2["<b>📦&nbsp;OrchardCore.Cms.KtuSaModule.csproj</b><br/><small>net8.0</small>"]
        click P1 "#ktusaheadlesscmsktusaheadlesscmscsproj"
        click P2 "#orchardcorecmsktusamoduleorchardcorecmsktusamodulecsproj"
    end
    subgraph current["OrchardCore.KtuSaTheme.csproj"]
        MAIN["<b>📦&nbsp;OrchardCore.KtuSaTheme.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#orchardcorektusathemeorchardcorektusathemecsproj"
    end
    P1 --> MAIN
    P2 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 813 |  |
| ***Total APIs Analyzed*** | ***813*** |  |

