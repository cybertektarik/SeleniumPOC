# How to Run Tests – Local, Browser (remote URL), and Perfecto

You can run the same tests in three ways. **No code changes** are needed—only environment variables and (for Perfecto) a token.

---

## 1. Run against **localhost** (free, on your machine)

Use this when your app is running locally (e.g. `http://localhost:8080`).

### Steps

1. Start your application on `http://localhost:8080` (or the URL you set in step 2b).
2. Set the environment variable so tests use the local URL:
   - **Option A – recommended:**  
     `USE_LOCAL=1`
   - **Option B – custom URL:**  
     `BASE_URL=http://localhost:8080/#/auth/login` (or your full login URL)
3. **Do not set** `RUN_REMOTE` (or set `RUN_REMOTE=0`) so the browser runs locally.
4. Run tests:
   ```powershell
   $env:USE_LOCAL = "1"
   dotnet test
   ```
   Or in CMD:
   ```cmd
   set USE_LOCAL=1
   dotnet test
   ```

**Summary:** Local browser + local app. Set `USE_LOCAL=1` (or `BASE_URL=...`). Leave `RUN_REMOTE` unset or `0`.

---

## 2. Run in **local browser** against **remote/live URL** (default)

Use this when you want to see the browser on your machine but hit the live/test environment (URLs from `Data/UserRoles_Set1.json` and `UserRoles_Set2.json`).

### Steps

1. **Do not set** `USE_LOCAL`, `BASE_URL`, or `RUN_REMOTE`. (Or set `USE_LOCAL=0` if you had it set before.)
2. Run tests:
   ```powershell
   dotnet test
   ```

**Summary:** Local browser + remote URL from JSON. No env vars needed (default).

---

## 3. Run on **Perfecto** (cloud browser)

Use this when you want tests to run in Perfecto’s cloud browser.

### Steps

1. Get your Perfecto security token (from your Perfecto project/setup).
2. Set environment variables:
   - `RUN_REMOTE=1` (or `RUN_REMOTE=true`)
   - `PERFECTO_TOKEN=<your-token>`
3. **Do not set** `USE_LOCAL` (or set `USE_LOCAL=0`) so the app URL stays the remote one from the JSON files.
4. Run tests:
   ```powershell
   $env:RUN_REMOTE = "1"
   $env:PERFECTO_TOKEN = "YOUR_PERFECTO_TOKEN"
   dotnet test
   ```
   Or in CMD:
   ```cmd
   set RUN_REMOTE=1
   set PERFECTO_TOKEN=YOUR_PERFECTO_TOKEN
   dotnet test
   ```

**Summary:** Perfecto cloud browser + remote URL from JSON. Set `RUN_REMOTE=1` and `PERFECTO_TOKEN`.

---

## Quick reference

| Goal                         | USE_LOCAL | BASE_URL | RUN_REMOTE | PERFECTO_TOKEN |
|-----------------------------|-----------|----------|------------|----------------|
| **Local app (localhost)**   | `1`       | (optional) | unset/0  | not needed     |
| **Local browser, remote URL** | unset/0 | unset    | unset/0   | not needed     |
| **Perfecto cloud**          | unset/0   | unset    | `1`       | required       |

### Optional: headless local browser

To run the **local** browser in headless mode (no window):

- Set `RUN_HEADLESS=1` (or `RUN_HEADLESS=true`).

Example (local browser, headless, local app):

```powershell
$env:USE_LOCAL = "1"
$env:RUN_HEADLESS = "1"
dotnet test
```

---

## Where URLs and users come from

- **URL:**  
  - If `USE_LOCAL=1` or `BASE_URL` is set → that URL is used.  
  - Otherwise → URL comes from `Data/UserRoles_Set1.json` or `Data/UserRoles_Set2.json` (based on scenario tags like `@feature2` / `@external`).
- **Users:**  
  Always from the same JSON files (Set1 or Set2 by tag). For local runs, ensure the usernames in those files exist in your local environment, or adjust the JSON for local-only users.

No need to edit the JSON files to switch between “localhost” and “remote”—use the env vars above.
