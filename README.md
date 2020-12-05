# DS4D_Group_15_Alarm
## Group Members
Luna Hu, Linsen Liu, Zhiyuan Du, Ruisheng Zheng, Ariel Hu

## Our Data Holder
Blackwood Homes & Care, founded in 1972, aims to help people live their lives to the fullest and provides high-quality care services, support, and housing to disabled people of all ages. Blackwood owns over 1500 tenancies across 29 mainland local authorities in Scotland.

## Data
Our alarm data is obtained from the Blackwood Homes & Care. We focus on analyzing current community alarm usage of Blackwood’s customers (disabled people in need of care services and support). The data contains customer’s sex, age, disability, city, home type, alarm type and start time.

## Project Name
Alarm Operator

## Format
Simulated management console game
Our porjetc is a simulated managemet console game that opens to the public.
The game simulates a real day in the life of a Blackwood alarm operator, who processes the alarms and responds to customer’s needs in a time manner.
Our delivered home message is to let people get a sense of the alarm usage of disabled people in Scotland and raise awareness of the needs of people with disabilities.

## About this Github Repository
This repository contains three aspects of the project, one of which is all the environment and scripts for the project game. One part is all the data used in the game, and the other part is all the images used in the game.

## Data Description
### Customers
114 customers in the dataset

### Top 3 Popular Alarm Services
* General Alarm (7991)
* Pull Cord Alarm (2531)
* Wrist Alarm (4748)

### Gender
* Female (53)
* Male (61)

### Age Groups
* Young ≤59 (74)
* Old ≥60 (40)

### Geographical regions of Scotland
* East: Edinburgh, Stirling
* West: Ayr, Glasgow, West
* North: Aberdeen, Dundee, Wishaw

### Disability types
* Physically Affected (28): Physical Injury, COPD, Spina Bifida, Arthritis, Asthma,Visual Impairment, Cancer, Amputee, Spina Bifida, Elderly Care/Support, Diabetes, Stroke, Lifelong Mobility Issues, Cerebral Palsy, Multiple Sclerosis, Brain Injury, Huntington's, Prone to Falls, Huntington's, Incontinence, Poor mobility, Parkinsons, Arthritis, Blind, Hearing, Impairment, Skin Issues, Multiple Sclerosis

* Physically Not Affected (5): Epilepsy, Mental Health Issues, Learning Difficulties, OCD,  Bi Polar Disorder

### Game Data Source
The user alarm data from 7:00 AM - 10:00 PM was extracted proportionally according to the user’s request for alarm service at different times of the day from the original Blackwood's dataset.

## Gamification
### Time
7: 00 A.M.-10: 00P.M. Game lasts for 4 minutes.

### 8 Types of Customers
1. Young Male Disability: Physical action not affected 
2. Young Female, Disability: Physical action not affected
3. Old Female Disability: Physical action affected
4. Old Male Disability: Physical action  affected
5. Young Male Disability: Physical action affected
6. Young Female Disability: Physical action affected
7. Old Female, Disability: Physical action not affected
8. Old Male Disability: Physical action not affected

### Service Request&  Provide Service
Customers approach the service desk with various alarm request. Player matches the request with the corresponding service package ASAP.
1. General Alarm
2. Wrist Alarm
3. Pull Cord Alarm
4. Othe Alarm

### Score
At the end of one day, the player passes the game if successfully responds to more than 70% of the customers; otherwise, the player fails. A rating (0-5) is displayed at the end of the game, based on how quickly the operator responds to the alarm request. 






