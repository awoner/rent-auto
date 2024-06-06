<details>
    <summary>Stress testing</summary>
  
| Concurrency | Availability (%) | Avg Response Time (secs) | Throughput (MB/sec) | Screenshots |
|-------------|------------------|--------------------------|---------------------|-------------|
| 10          | 100.00           | 0.16                     | 34.05               | ![photo_1_2024-06-06_16-37-29](https://github.com/awoner/rent-auto/assets/33530161/306c799c-1003-4d7b-b8aa-76327fc5e737) |
| 25          | 100.00           | 0.28                     | 24.70               | ![photo_4_2024-06-06_16-37-29](https://github.com/awoner/rent-auto/assets/33530161/e130d674-f370-4b07-a873-d9a0915a2100) |
| 50          | 100.00           | 0.88                     | 36.46               | ![photo_3_2024-06-06_16-37-29](https://github.com/awoner/rent-auto/assets/33530161/55cb5357-67de-4a76-b7ee-d92c4be02b0d) |
| 76 _(because my machine can't process more)_         | 20.41            | 0.32                     | 74.97               | ![photo_4_2024-06-06_16-37-29](https://github.com/awoner/rent-auto/assets/33530161/7b4ee8d8-b6a3-4f4f-9fe5-0d3255268824) |
</details>

<details>
    <summary>Application metrics</summary>
  
On application started cronjob running and every hour request UAH/USD currency rate and send it to GoogleAnalytics via GAMP

Results:
    https://lookerstudio.google.com/u/0/explorer/dd5dfb21-2d3e-4173-bd00-f1a0feaaaa9a
    ![image](https://github.com/awoner/rent-auto/assets/33530161/e8a5defe-3792-4ad6-a11d-e9ebd551e5e5)
</details>
