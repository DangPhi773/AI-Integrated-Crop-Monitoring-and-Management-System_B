# CMMS Postman API Tests

Bộ test integration cho CMMS Web API, phủ các module **không có** trong file `UnitTest cmms.xlsx`:
Plant AI · Contract · Payment · Expense · IoT Device + Sensor · Notification · Disease Report · Recommendation Task · Statistics · Weather · Maps.

## Files

| File | Mô tả |
|---|---|
| `CMMS_API_Tests.postman_collection.json` | Collection v2.1 — 14 folder, ~70 request, mỗi request có `tests` script |
| `CMMS_Local.postman_environment.json` | Environment biến: `baseUrl`, account 3 role, token, IDs |

## Cấu trúc

| Folder | Phủ |
|---|---|
| `00_Auth` | Login 3 role + lưu token, register, change-password, wrong-password, weak-password |
| `01_Plant_AI` | Analyze ảnh cây (normal / thiếu ảnh / no-auth) |
| `02_Contract` | CRUD contract + duplicate check + invalid BankBin + role check |
| `03_Payment` | Pending bill, upload payment, get by id, get my, get by month |
| `04_Expense` | CRUD expense + boundary (zero amount) + summary |
| `05_IoT_Devices` | Create (lưu API key) / CRUD / rotate-key / boundary lat-lng |
| `06_IoT_Data_and_Sensor` | Receive sensor (normal / no-key / boundary alert / abnormal range) + history/latest |
| `07_Notification` | Get mine / unread-count / mark read / mark all read |
| `08_Disease_Reports` | Worker tạo report → Owner assign → Specialist diagnosis → Get/Delete |
| `09_Recommendation_Tasks` | Create + GetAll |
| `10_Statistics` | Yield by crop/season/plot/summary |
| `11_Weather` | Current / Forecast / By farm |
| `12_Maps` | Geocode / Reverse / Update farm coords |
| `13_Cleanup_IoT_Device` | Xoá device đã tạo |

## Setup

1. **Import vào Postman**
   - File → Import → chọn 2 file `.json` ở folder này
2. **Chọn environment** `CMMS Local` (góc phải trên)
3. **Điền account thật** vào env (Owner / Worker / Specialist đã có trong DB):
   ```
   emailOwner, passwordOwner
   emailWorker, passwordWorker
   emailSpecialist, passwordSpecialist
   ```
4. **Điền sẵn 1 số ID** (lấy từ DB hoặc Swagger):
   ```
   farmId, plotId, bedId, seasonId
   ownerUserId, workerUserId, specialistUserId
   ```
   Các ID còn lại (`contractId`, `deviceId`, `paymentId`, `reportId`, `diagnosisId`, …) **được tự động set** bởi test script sau khi Create chạy thành công.

## Run

### Cách 1 — Collection Runner (UI)
1. Click collection `CMMS API Tests` → tab **Runner**
2. Chọn env `CMMS Local`
3. Chạy theo thứ tự folder (00 → 13)
4. Xem kết quả pass/fail mỗi assertion

### Cách 2 — Newman CLI (CI/CD)
```bash
npm install -g newman newman-reporter-htmlextra

newman run CMMS_API_Tests.postman_collection.json \
  -e CMMS_Local.postman_environment.json \
  -r cli,htmlextra \
  --reporter-htmlextra-export ./report.html
```

## Lưu ý quan trọng

- **`01_Plant_AI` và `08_Disease_Reports`** dùng form-data file upload → trước khi chạy phải **chọn file ảnh thật** vào field `Image` / `images` (Postman không lưu đường dẫn file qua import).
- **`03_Payment / Upload Payment`** tương tự, cần chọn file bill (pdf/png).
- **`06_IoT_Data_and_Sensor`** cần chạy SAU `05_IoT_Devices / Create IoT Device` để có `deviceApiKey`.
- **`08_Disease_Reports / Assign`** cần `reportId` từ Create + `specialistUserId` trong env.
- **`13_Cleanup`** chạy sau cùng để xoá device test.

## Phân loại test case (theo guideline FPT đã dùng trong Excel)

| Loại | Có trong collection |
|---|---|
| **Normal (N)** | Happy path mỗi endpoint |
| **Boundary (B)** | Zero amount, lat=999, weak password 8-char, duplicate contract, … |
| **Abnormal (A)** | Wrong password, missing image, no-auth, no-X-Device-Key, out-of-range temp, wrong role (403), … |

Mỗi request có **`pm.test(...)`** kiểm tra:
- HTTP status code
- Shape response (`success`, `data`, `errors`)
- Business rule (vd: `isAlert=true` khi nhiệt độ vượt ngưỡng)
- Auto-save ID vào env cho test sau

## Mở rộng

Nếu muốn thêm test cho các module **đã có** trong Excel (Farm, Plot, Crop, …), copy 1 folder bất kỳ, đổi URL và body. Test scripts có thể reuse nguyên.
