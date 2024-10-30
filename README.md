# About app
An Asp.NET Core app that represents a bank.

# Hosting
The app is hosted here: [https://bankweb24.azurewebsites.net](https://bankweb24.azurewebsites.net)
> [!NOTE]
> If you want to run the app locally, you would have to add a connection string and do a migration.

# How to use the program?

# Login and Register
You will first be faced with 2 buttons: Login and Register. When you press "Register" it takes you to a page where you need to type in your credentials. They are the following: EGN, First name, Last name, Email adress and your choice of a 4-digit PIN code. You need to remeber your PIN and your IBAN. Once you fill the boxes with a valid information it will take you to a page where a random card number is generated for you. You need to remember your card number. Then you can press "Login". This takes you to a page where you need to type in your card number and PIN, that you used to register with. Upon entering valid information you will be taken to the Bank Menu page.

# Bank Menu
The Bank Menu has 8 buttons. They are: Show Balance, Withdraw Money. Deposit Money, Transfer Money, Show Credit Info, Take Credit, Pay Credit, Logout.

# Show Balance
Upon pressing this button you will be taken to a page in which you can see your balance. You can press "Go Back" at any time to return to the Bank Menu page.

# Withdraw Money
Upon pressing this button you will be taken to a page with a box, in which you can type in the amount you wish to withdraw. Then by pressing "Submit" you can withdraw the amount you wrote if the number entered is valid. You can press "Go Back" at any time to return to the Bank Menu page.

# Deposit Money
Upon pressing this button you will be taken to a page, where you can type in the amount you wish to deposit. Then by pressing "Submit" you can deposit the amount you wrote if the number entered is valid. You can press "Go Back" at any time to return to the Bank Menu page.

# Transfer Money
Upon pressing this button you will be taken to a page, in which you can type in the IBAN of the person you are sending money to and the amount of money you wish to trasnfer. Then by pressing "Submit" you can transer the amount you wrote to the IBAN you wrote. Upon entering valid information the money will be sent. You can press "Go Back" at any time to return to the Bank Menu page.

# Show Credit Info
Upon pressing this button you will be taken to a page, in which you can see if you have an existing credit and if you have one - you can see the information about the credit. You can press "Go Back" at any time to return to the Bank Menu page.

# Take Credit
Upon pressing this button you will be taken to a page with 3 options for a credit, each one on a different button. You can take credit by pressing one of the buttons. If you don't have an existing credit and you press one of the buttons for credit you will be taken to a page with a message that you have succesfully taken a credit and your updated balance. If you have an existing credit and you press on of the buttons for a credit, you will not be able to take another credit until you pay your current one and an error message will be displayed instead. You can press "Go Back" at any time to return to the Bank Menu page.

# Pay Credit
Upon pressing this button you will be taken to a page with a message, according to your credit paying status. You can press "Go Back" at any time to return to the Bank Menu page.

# Logout
Upon pressing this button you will be taken to the starter page. There you can Login or Register.
