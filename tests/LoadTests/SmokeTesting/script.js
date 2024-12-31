import http from 'k6/http';
import { check, sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

export const options = {
    vus: 3, // Key for Smoke test. Keep it at 2, 3, max 5 VUs
    duration: '10s', // This can be shorter or just a few iterations
};

export default () => {
    const urlRes = http.get('https://test-api.k6.io');
    sleep(1);
    // MORE STEPS
    // Here you can have more steps or complex script
    // Step1
    // Step2
    // etc.
};

export function handleSummary(data) {
    return {
        "script.html": htmlReport(data),
        stdout: textSummary(data, { indent: " ", enableColors: true }),
    };
}