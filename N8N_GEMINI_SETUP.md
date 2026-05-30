# Chuyển hướng AI sang Gemini + n8n

## 1. Backend ASP.NET

Thay file:

```text
CMMS.BLL/Services/PlantAnalysisService.cs
```

bằng file trong patch này.

Thêm vào `CMMS.WebAPI/appsettings.json`:

```json
"N8nAi": {
  "WebhookUrl": "http://localhost:5678/webhook/crop-diagnosis",
  "Secret": "change-this-secret",
  "TimeoutSeconds": 60
}
```

Nếu không dùng ONNX nữa, có thể xóa package:

```xml
<PackageReference Include="Microsoft.ML.OnnxRuntime" Version="1.20.1" />
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.6" />
```

khỏi `CMMS.BLL.csproj`. Không xóa cũng không sao.

## 2. n8n workflow

Tạo workflow:

```text
Webhook -> Gemini Analyze Image / HTTP Request Gemini -> Code/Set format JSON -> Respond to Webhook
```

Webhook:

```text
Method: POST
Path: crop-diagnosis
Response mode: Using Respond to Webhook node
```

Backend sẽ gửi multipart/form-data gồm:

```text
image, farmId, plotId, bedId, plantName, growthStage,
temperature, airHumidity, soilMoisture, lightIntensity, weatherCondition
```

## 3. Prompt Gemini

Dùng prompt:

```text
You are an agricultural plant disease diagnosis assistant.
Analyze the uploaded crop leaf image and the provided environmental context.
Return ONLY valid JSON. Do not wrap in markdown.

Context:
- Plant name: {{$json.body.plantName}}
- Growth stage: {{$json.body.growthStage}}
- Temperature: {{$json.body.temperature}}
- Air humidity: {{$json.body.airHumidity}}
- Soil moisture: {{$json.body.soilMoisture}}
- Light intensity: {{$json.body.lightIntensity}}
- Weather condition: {{$json.body.weatherCondition}}

Required JSON schema:
{
  "disease": "Vietnamese disease name",
  "diseaseCode": "UPPER_SNAKE_CASE_CODE",
  "confidence": 0.0,
  "severity": "Thấp | Trung bình | Cao",
  "description": "Natural Vietnamese diagnosis for farmers",
  "symptoms": ["..."],
  "solutions": ["..."],
  "treatmentSteps": ["..."],
  "topPredictions": [
    { "label": "DISEASE_CODE", "display_name": "English disease name", "confidence": 0.0 }
  ],
  "english": {
    "disease": "English disease name",
    "description": "English diagnosis",
    "severity": "Low | Medium | High",
    "symptoms": ["..."],
    "care_suggestions": ["..."],
    "treatment_steps": ["..."]
  },
  "vietnamese": {
    "disease": "Vietnamese disease name",
    "description": "Natural Vietnamese diagnosis",
    "severity": "Thấp | Trung bình | Cao",
    "symptoms": ["..."],
    "care_suggestions": ["..."],
    "treatment_steps": ["..."]
  }
}

Rules:
- If the image is unclear, lower the confidence.
- If disease cannot be identified, use "Không xác định" and explain that the image should be retaken.
- Make Vietnamese farmer-friendly and practical.
- Do not invent pesticide dosage.
```

## 4. Test

Chạy n8n ở port 5678, bật workflow active, rồi test Swagger:

```text
POST /api/Plant/analyze
```
