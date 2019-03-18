# Umbraco-OCR
An OCR Content App for Umbraco

![https://github.com/prjseal/Umbraco-OCR/blob/master/images/logo.jpg?raw=true](https://github.com/prjseal/Umbraco-OCR/blob/master/images/logo.jpg?raw=true)

You need the following app settings in your web.config file:
                        
<add key="OcrSubscriptionKey" value="azure-subscription-key-here" />
<add key="OcrApiUrl" value="https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr" />

Please make sure you have a valid subscription key for the Azure Vision API in the 
app settings of the web.config file. Also check that the ocr api endpoint is correct.
                        
To find out how to get a subscription key, visit this page:
https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/vision-api-how-to-topics/howtosubscribe