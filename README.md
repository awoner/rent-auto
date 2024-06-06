<details>
    <summary>Stress testing</summary>
  
    | Concurrency | Availability (%) | Avg Response Time (secs) | Throughput (MB/sec) |
    |-------------|------------------|--------------------------|---------------------|
    | 10          | 100.00           | 0.16                     | 34.05               |
    | 25          | 100.00           | 0.28                     | 24.70               |
    | 50          | 100.00           | 0.88                     | 36.46               |
    | 76 _(because my machine can't process more)_         | 20.41            | 0.32                     | 74.97               |
</details>

<details>
    <summary>Application metrics</summary>
  
    On application started cronjob running and every hour request UAH/USD currency rate and send it to GoogleAnalytics via GAMP
    
    Results:
    https://lookerstudio.google.com/u/0/explorer/dd5dfb21-2d3e-4173-bd00-f1a0feaaaa9a
    ![image](https://github.com/awoner/rent-auto/assets/33530161/e8a5defe-3792-4ad6-a11d-e9ebd551e5e5)
</details>