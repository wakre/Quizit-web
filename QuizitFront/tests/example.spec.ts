import { test, expect } from '@playwright/test';

//testing the quiztaking mode
test('play a quiz regardless of its questions', async ({ page }) => {
  await page.goto('http://localhost:5173');

  // Go to quiz list
  await page.getByRole('link', { name: /view available quizzes/i }).click();

  // Open first quiz
  await page.getByRole('button', { name: /view details/i }).first().click();

  // Start quiz
  await page.getByRole('button', { name: /take quiz/i }).click();

  // Loop through quiz questions until "Submit Quiz" appears
  while (true) {
    // Wait for question to load
    await expect(page.getByText(/question/i)).toBeVisible();

    // Select the first available radio option
    const options = page.getByRole('radio');

    const count = await options.count();
   // expect(count).toBeGreaterThan(0);

    await options.nth(0).check();

    // If final question → "Submit Quiz" appears instead of "Next"
    const isSubmitVisible = await page
      .getByRole('button', { name: /submit quiz/i })
      .isVisible()
      .catch(() => false);

    if (isSubmitVisible) {
      await page.getByRole('button', { name: /submit quiz/i }).click();
      break;
    }

    // Otherwise click "Next"
    await page.getByRole('button', { name: /next/i }).click();
  }

  // Check score screen
  await expect(page.getByText(/your score/i)).toBeVisible();

  // Return home
  await page.getByRole('button', { name: /home/i }).click();
});



test('create quiz flow', async ({ page }) => {
   await page.goto('http://localhost:5173');
  // Go to Create Your Own Quiz → triggers redirect to login
  await page.getByRole('link', { name: 'Create Your Own Quiz' }).click();

  // Login
  await page.locator('input[type="email"]').fill('saidah@gmail.com');
  await page.locator('input[type="password"]').fill('saidah123');
  await page.getByRole('button', { name: /login/i }).click();

  // Wait for create quiz page (exact heading exists)
  await expect(page.getByRole('heading', { name: 'Create Quiz' })).toBeVisible();

  await page.locator('input[type="text"]').first().fill('math game'); // title
  await page.locator('textarea').fill('easy math quiz');
  await page.getByRole('combobox').selectOption('4');
  await page.getByRole('button', { name: /create quiz/i }).click();


  // Add question 1
  await page.getByRole('textbox').first().fill('5+5');
  await page.getByRole('textbox').nth(1).fill('10');
  await page.getByRole('textbox').nth(2).fill('15');

  await page.getByRole('button', { name: '+ Add Option' }).click();
  await page.getByRole('textbox').nth(3).fill('13');

  await page.getByRole('radio', { name: '1' }).check();
  await page.getByRole('button', { name: 'Add Question' }).click();

  // Add question 2
  await page.getByRole('textbox').first().fill('10+10');
  await page.getByRole('textbox').nth(1).fill('15');
  await page.getByRole('textbox').nth(2).fill('11');

  await page.getByRole('button', { name: '+ Add Option' }).click();
  await page.getByRole('textbox').nth(3).fill('20');

  await page.getByRole('radio', { name: '3' }).check();
  await page.getByRole('button', { name: 'Add Question' }).click();

  await page.getByRole('button', { name: 'Finish' }).click();

  // Validate quiz appears in list
  await page.getByRole('link', { name: 'View Available Quizzes' }).click();
  await expect(page.locator('.card-title', { hasText: 'math game' }).first()).toBeVisible();


});
