# 🎮 Unity Project Setup & Collaboration Guide

Welcome to the Unity project!
This document explains how to clone, open, and contribute to the project safely using Git and GitHub.

---

## Overview

This project uses Git for version control and follows Unity’s recommended `.gitignore` setup.
Only essential files (like assets, scripts, scenes, and settings) are tracked — everything Unity can regenerate is ignored.

### Tracked by Git
- Assets/ — scripts, prefabs, textures, scenes, etc.
- Packages/ — defines which Unity packages are used.
- ProjectSettings/ — global settings like layers, input, and graphics.
- .meta files — Unity’s asset link data (do not delete these).

### Ignored by Git
- Library/, Temp/, Obj/, Build/, Logs/
- *.sln, *.csproj, and other auto-generated IDE files
- UserSettings/ — local editor preferences

Unity automatically regenerates all ignored folders when the project is opened.

---

## Getting Started

### 1. Clone the Repository
git clone https://github.com/<YourUsername>/<YourRepoName>.git
cd <YourRepoName>

### 2. Open the Project in Unity
1. Open Unity Hub
2. Click “Add Project”
3. Select the cloned folder
4. Unity will automatically rebuild the project (may take a few minutes)

The project will now be ready to play and edit.

---

## Collaboration Workflow

### Create a New Branch for Your Work
git checkout -b feature-your-feature-name

### Make Your Changes
Modify scripts, prefabs, scenes, etc. Avoid editing the same scene or prefab as another teammate to reduce merge conflicts.

### Commit and Push
git add .
git commit -m "Describe your changes here"
git push origin feature-your-feature-name

### Open a Pull Request
1. Go to the repo on GitHub
2. Click “Compare & pull request”
3. Request a review and merge once approved

---

## Recommended Unity Settings

To make Git work smoothly with Unity assets:

1. Go to Edit → Project Settings → Editor
2. Under Asset Serialization, set Mode: Force Text
3. Under Version Control, set Mode: Visible Meta Files

This ensures Unity stores assets in text format (so Git can merge them) and includes .meta files (to keep asset links intact).

---

## Tips for Teamwork

- Always pull before pushing to keep your local branch up to date.
- Commit often, but with purpose — each commit should represent one logical change.

---

## Optional: Faster Loads

First-time project opens can take a few minutes while Unity rebuilds the Library. For larger teams, consider using Unity’s Cache Server (Accelerator) to speed this up.

---

## Summary

| Action | Description |
|--------|-------------|
| Clone the repo | git clone <url> |
| Open in Unity Hub | Unity rebuilds missing folders |
| Create a feature branch | Work in isolation |
| Commit and push | Share your progress |
| Pull request | Get code reviewed and merged |

