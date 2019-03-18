  ____   _____ _____     _____            _             _                          
 / __ \ / ____|  __ \   / ____|          | |           | |       /\                
| |  | | |    | |__) | | |     ___  _ __ | |_ ___ _ __ | |_     /  \   _ __  _ __  
| |  | | |    |  _  /  | |    / _ \| '_ \| __/ _ \ '_ \| __|   / /\ \ | '_ \| '_ \ 
| |__| | |____| | \ \  | |___| (_) | | | | ||  __/ | | | |_   / ____ \| |_) | |_) |
 \____/ \_____|_|  \_\  \_____\___/|_| |_|\__\___|_| |_|\__| /_/    \_\ .__/| .__/ 
                                                                      | |   | |    
                                                                      |_|   |_|    
___________________________________________________________________________________
                                              
You need the following app settings in your web.config file:
                        
<add key="OcrSubscriptionKey" value="azure-subscription-key-here" />
<add key="OcrApiUrl" value="https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr" />

Please make sure you have a valid subscription key for the Azure Vision API in the 
app settings of the web.config file. Also check that the ocr api endpoint is correct.
                        
To find out how to get a subscription key, visit this page:
https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/vision-api-how-to-topics/howtosubscribe