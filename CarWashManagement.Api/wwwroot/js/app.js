let customers = [];
let vehicles = [];
let stations = [];
let transactions = [];

const stationGrid = document.getElementById("stationGrid");
const customerList = document.getElementById("customerList");
const activeWashList = document.getElementById("activeWashList");
const transactionTableBody = document.getElementById("transactionTableBody");

const availableCount = document.getElementById("availableCount");
const occupiedCount = document.getElementById("occupiedCount");
const customerCount = document.getElementById("customerCount");
const transactionCount = document.getElementById("transactionCount");

const vehicleCustomer = document.getElementById("vehicleCustomer");
const washCustomer = document.getElementById("washCustomer");
const washVehicle = document.getElementById("washVehicle");

const customerForm = document.getElementById("customerForm");
const vehicleForm = document.getElementById("vehicleForm");
const startWashForm = document.getElementById("startWashForm");

const customerMessage = document.getElementById("customerMessage");
const vehicleMessage = document.getElementById("vehicleMessage");
const washMessage = document.getElementById("washMessage");

const refreshButton = document.getElementById("refreshButton");

const stationStatusNames = {
    0: "Available",
    1: "Occupied",
    2: "OutOfService",
    3: "Maintenance"
};

const washProgramNames = {
    0: "Basic",
    1: "Premium",
    2: "Deluxe"
};

const transactionStatusNames = {
    0: "In Progress",
    1: "Completed",
    2: "Cancelled"
};

async function loadDashboard() {
    try {
        const responses = await Promise.all([
            fetch("/api/Customers"),
            fetch("/api/Vehicle"),
            fetch("/api/WashStation"),
            fetch("/api/WashTransaction")
        ]);

        for (const response of responses) {
            if (!response.ok) {
                throw new Error(
                    `Request failed: ${response.url} (${response.status})`
                );
            }
        }

        customers = await responses[0].json();
        vehicles = await responses[1].json();
        stations = await responses[2].json();
        transactions = await responses[3].json();

        populateCustomerSelectors();
        renderStations();
        renderCustomers();
        renderTransactions();
        renderActiveWashes();
        updateSummary();
    }
    catch (error) {
        console.error(error);

        stationGrid.innerHTML =
            `<p class="empty-message">${error.message}</p>`;
    }
}

function populateCustomerSelectors() {
    vehicleCustomer.innerHTML =
        '<option value="">Select customer</option>';

    washCustomer.innerHTML =
        '<option value="">Select customer</option>';

    customers.forEach(customer => {
        const name =
            `${customer.firstName} ${customer.lastName}`;

        const vehicleOption =
            document.createElement("option");

        vehicleOption.value = customer.id;
        vehicleOption.textContent = name;

        vehicleCustomer.appendChild(vehicleOption);

        const washOption =
            document.createElement("option");

        washOption.value = customer.id;
        washOption.textContent = name;

        washCustomer.appendChild(washOption);
    });
}

function populateVehicleSelector(customerId) {
    washVehicle.innerHTML =
        '<option value="">Select vehicle</option>';

    if (!customerId) {
        washVehicle.disabled = true;
        return;
    }

    const customerVehicles = vehicles.filter(
        vehicle => vehicle.customerId === customerId
    );

    customerVehicles.forEach(vehicle => {
        const option =
            document.createElement("option");

        option.value = vehicle.licensePlate;

        option.textContent =
            `${vehicle.licensePlate} - ${vehicle.make} ${vehicle.model}`;

        washVehicle.appendChild(option);
    });

    washVehicle.disabled = customerVehicles.length === 0;

    if (customerVehicles.length === 0) {
        washVehicle.innerHTML =
            '<option value="">No vehicles registered</option>';
    }
}

function renderStations() {
    stationGrid.innerHTML = "";

    if (stations.length === 0) {
        stationGrid.innerHTML =
            '<p class="empty-message">No stations found.</p>';

        return;
    }

    [...stations]
        .sort((a, b) =>
            a.stationNumber - b.stationNumber
        )
        .forEach(station => {
            const status =
                getStationStatusName(station.status);

            const card =
                document.createElement("div");

            card.className = "station-card";

            card.innerHTML = `
                <div class="station-card-header">
                    <span class="station-number">
                        Station ${station.stationNumber}
                    </span>

                    <span class="station-status ${getStationStatusClass(status)}">
                        ${status}
                    </span>
                </div>

                <div class="station-meta">
                    ${station.isActive
                        ? "Station active"
                        : "Station disabled"}
                </div>
            `;

            stationGrid.appendChild(card);
        });
}

function renderCustomers() {
    customerList.innerHTML = "";

    if (customers.length === 0) {
        customerList.innerHTML =
            '<p class="empty-message">No customers yet.</p>';

        return;
    }

    customers.forEach(customer => {
        const card = document.createElement("div");

        card.className = "customer-card";

        const customerVehicles = vehicles.filter(
            vehicle => vehicle.customerId === customer.id
        );

        const vehicleRows = customerVehicles.length === 0
            ? '<p class="empty-message">No vehicles</p>'
            : customerVehicles
                .map(vehicle => `
                    <div class="vehicle-row">
                        <span>
                            ${vehicle.licensePlate}
                            - ${vehicle.make} ${vehicle.model}
                        </span>

                        <button
                            class="delete-button"
                            onclick="deleteVehicle('${vehicle.licensePlate}')">
                            Remove Vehicle
                        </button>
                    </div>
                `)
                .join("");

        card.innerHTML = `
    <div class="customer-header">
        <div>
            <strong>
                ${customer.firstName} ${customer.lastName}
            </strong>

            <span>
                Email: ${customer.email}
            </span>

            <span>
                Phone: ${customer.phoneNumber ?? "Not provided"}
            </span>

            <span>
                GUID: ${customer.id}
            </span>
        </div>

        <button
            class="delete-button"
            onclick="deleteCustomer('${customer.id}')">
            Delete Customer
        </button>
    </div>

    <div class="customer-vehicles">
        ${vehicleRows}
    </div>
`;

        customerList.appendChild(card);
    });
}
async function deleteVehicle(licensePlate) {
    const confirmed = confirm(
        `Remove vehicle ${licensePlate}?`
    );

    if (!confirmed) {
        return;
    }

    try {
        const response = await fetch(
            `/api/Vehicle/${encodeURIComponent(licensePlate)}`,
            {
                method: "DELETE"
            }
        );

        if (!response.ok) {
            alert("The vehicle could not be deleted.");
            return;
        }

        await loadDashboard();
    }
    catch (error) {
        console.error(error);
        alert("Could not connect to the API.");
    }
}
async function deleteCustomer(customerId) {
    const confirmed = confirm(
        "Delete this customer?"
    );

    if (!confirmed) {
        return;
    }

    try {
        const response = await fetch(
            `/api/Customers/${customerId}`,
            {
                method: "DELETE"
            }
        );

        if (response.status === 409) {
            alert(
                "Remove all vehicles from this customer first."
            );

            return;
        }

        if (!response.ok) {
            alert("The customer could not be deleted.");
            return;
        }

        await loadDashboard();
    }
    catch (error) {
        console.error(error);
        alert("Could not connect to the API.");
    }
}
function renderTransactions() {
    transactionTableBody.innerHTML = "";

    if (transactions.length === 0) {
        transactionTableBody.innerHTML = `
            <tr>
                <td colspan="7">
                    No transactions yet.
                </td>
            </tr>
        `;

        return;
    }

    const recentTransactions =
        [...transactions]
            .sort(
                (a, b) =>
                    new Date(b.startedAt) -
                    new Date(a.startedAt)
            )
            .slice(0, 10);

    recentTransactions.forEach(transaction => {
        const customer = customers.find(
            customer =>
                customer.id === transaction.customerId
        );

        const customerName = customer
            ? `${customer.firstName} ${customer.lastName}`
            : "Unknown customer";

        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${customerName}</td>
            <td>${transaction.licensePlate}</td>
            <td>${getWashProgramName(transaction.washProgram)}</td>
            <td>${transaction.stationNumber}</td>
            <td>${getTransactionStatusName(transaction.status)}</td>
            <td>${formatDate(transaction.startedAt)}</td>
            <td>${formatDate(transaction.completedAt)}</td>
        `;

        transactionTableBody.appendChild(row);
    });
}

function renderActiveWashes() {
    activeWashList.innerHTML = "";

    const activeTransactions = transactions.filter(
        transaction =>
            transaction.status === 0 ||
            transaction.status === "InProgress"
    );

    if (activeTransactions.length === 0) {
        activeWashList.innerHTML =
            '<p class="empty-message">No washes currently in progress.</p>';

        return;
    }

    activeTransactions.forEach(transaction => {
        const card = document.createElement("div");

        card.className = "activity-card";

        card.innerHTML = `
            <strong>${transaction.licensePlate}</strong>

            <span>
                Station ${transaction.stationNumber}
                · ${getWashProgramName(transaction.washProgram)}
            </span>

            <span>
                Remaining:
                <strong id="timer-${transaction.id}">
                    ${formatRemainingTime(
    getRemainingMilliseconds(transaction)
)}
                </strong>
            </span>

            <button
                class="small-button"
                onclick="completeWash('${transaction.id}')">
                Complete Wash
            </button>
        `;

        activeWashList.appendChild(card);
    });
}
function getWashDurationMilliseconds(program) {
    switch (Number(program)) {
        case 0:
            return 5 * 60 * 1000;

        case 1:
            return 8 * 60 * 1000;

        case 2:
            return 12 * 60 * 1000;

        default:
            return 5 * 60 * 1000;
    }
}
let isRefreshingAfterWash = false;

async function updateWashTimers() {
    const activeTransactions = transactions.filter(
        transaction =>
            transaction.status === 0 ||
            transaction.status === "InProgress"
    );

    let washHasExpired = false;

    activeTransactions.forEach(transaction => {
        const timer = document.getElementById(
            `timer-${transaction.id}`
        );

        if (!timer) {
            return;
        }

        const remainingTime =
            getRemainingMilliseconds(transaction);

        if (remainingTime <= 0) {
            timer.textContent = "Finishing...";
            washHasExpired = true;
        }
        else {
            timer.textContent =
                formatRemainingTime(remainingTime);
        }
    });

    if (washHasExpired && !isRefreshingAfterWash) {
        isRefreshingAfterWash = true;

        setTimeout(async () => {
            await loadDashboard();
            isRefreshingAfterWash = false;
        }, 10000);
    }
}
function getRemainingMilliseconds(transaction) {
    if (!transaction.startedAt) {
        return 0;
    }

    let startedAtValue = String(transaction.startedAt);

    const hasTimezone =
        startedAtValue.endsWith("Z") ||
        /[+-]\d{2}:\d{2}$/.test(startedAtValue);

    if (!hasTimezone) {
        startedAtValue += "Z";
    }

    const startedAt =
        new Date(startedAtValue).getTime();

    if (Number.isNaN(startedAt)) {
        return 0;
    }

    const duration =
        getWashDurationMilliseconds(
            transaction.washProgram
        );

    const finishTime =
        startedAt + duration;

    return finishTime - Date.now();
}
function formatRemainingTime(remaining) {
    if (remaining <= 0) {
        return "Finishing...";
    }

    const minutes =
        Math.floor(remaining / 60000);

    const seconds =
        Math.floor(
            (remaining % 60000) / 1000
        );

    return `${minutes}:${seconds
        .toString()
        .padStart(2, "0")}`;
}
async function completeWash(transactionId) {
    try {
        const response = await fetch(
            `/api/WashTransaction/${transactionId}/complete`,
            {
                method: "POST"
            }
        );

        if (!response.ok) {
            alert("The wash could not be completed.");
            return;
        }

        await loadDashboard();
    }
    catch (error) {
        console.error(error);

        alert("Could not connect to the API.");
    }
}

function updateSummary() {
    availableCount.textContent =
        stations.filter(
            station =>
                getStationStatusName(station.status) ===
                "Available"
        ).length;

    occupiedCount.textContent =
        stations.filter(
            station =>
                getStationStatusName(station.status) ===
                "Occupied"
        ).length;

    customerCount.textContent =
        customers.length;

    transactionCount.textContent =
        transactions.length;
}

async function addCustomer(event) {
    event.preventDefault();

    hideMessage(customerMessage);

    const body = {
        firstName:
            document.getElementById("firstName")
                .value.trim(),

        lastName:
            document.getElementById("lastName")
                .value.trim(),

        email:
            document.getElementById("email")
                .value.trim(),

        phoneNumber:
            document.getElementById("phoneNumber")
                .value.trim() || null
    };

    try {
        const response =
            await fetch("/api/Customers", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(body)
            });

        if (!response.ok) {
            const message =
                await response.text();

            showMessage(
                customerMessage,
                message || "Could not add customer.",
                false
            );

            return;
        }

        customerForm.reset();

        showMessage(
            customerMessage,
            "Customer added successfully.",
            true
        );

        await loadDashboard();
    }
    catch (error) {
        console.error(error);

        showMessage(
            customerMessage,
            "Could not connect to the API.",
            false
        );
    }
}

async function addVehicle(event) {
    event.preventDefault();

    hideMessage(vehicleMessage);

    const body = {
        customerId: vehicleCustomer.value,

        licensePlate:
            document
                .getElementById("vehicleLicensePlate")
                .value
                .trim()
                .toUpperCase(),

        make:
            document
                .getElementById("vehicleMake")
                .value
                .trim(),

        model:
            document
                .getElementById("vehicleModel")
                .value
                .trim(),

        year:
            Number(
                document
                    .getElementById("vehicleYear")
                    .value
            )
    };

    try {
        const response =
            await fetch("/api/Vehicle", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(body)
            });

        if (!response.ok) {
            const message =
                await response.text();

            showMessage(
                vehicleMessage,
                message || "Could not add vehicle.",
                false
            );

            return;
        }

        vehicleForm.reset();

        showMessage(
            vehicleMessage,
            "Vehicle added successfully.",
            true
        );

        await loadDashboard();
    }
    catch (error) {
        console.error(error);

        showMessage(
            vehicleMessage,
            "Could not connect to the API.",
            false
        );
    }
}

async function startWash(event) {
    event.preventDefault();

    hideMessage(washMessage);

    const body = {
        customerId: washCustomer.value,
        licensePlate: washVehicle.value,
        washProgram:
            Number(
                document.getElementById("washProgram").value
            )
    };

    try {
        const response =
            await fetch(
                "/api/WashTransaction/start",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                }
            );

        if (response.status === 404) {
            showMessage(
                washMessage,
                "Vehicle was not found for this customer.",
                false
            );

            return;
        }

        if (response.status === 409) {
            const message = await response.text();

            showMessage(washMessage,message,false);
            return;
        }

        if (!response.ok) {
            const message =
                await response.text();

            showMessage(
                washMessage,
                message || "Could not start wash.",
                false
            );

            return;
        }

        const transaction =
            await response.json();

        showMessage(
            washMessage,
            `Wash started on Station ${transaction.stationNumber}.`,
            true
        );

        startWashForm.reset();

        washVehicle.innerHTML =
            '<option value="">Select vehicle</option>';

        washVehicle.disabled = true;

        await loadDashboard();
    }
    catch (error) {
        console.error(error);

        showMessage(
            washMessage,
            "Could not connect to the API.",
            false
        );
    }
}

function showMessage(element, message, success) {
    element.textContent = message;

    element.className =
        success
            ? "message message-success"
            : "message message-error";
}

function hideMessage(element) {
    element.className = "message hidden";
}

function getStationStatusName(status) {
    if (typeof status === "string") {
        return status;
    }

    return stationStatusNames[status] ?? "Unknown";
}

function getWashProgramName(program) {
    if (typeof program === "string") {
        return program;
    }

    return washProgramNames[program] ?? "Unknown";
}

function getTransactionStatusName(status) {
    if (typeof status === "string") {
        return status === "InProgress"
            ? "In Progress"
            : status;
    }

    return transactionStatusNames[status] ?? "Unknown";
}

function getStationStatusClass(status) {
    return `status-${status
        .toLowerCase()
        .replaceAll(" ", "")}`;
}

function formatDate(value) {
    if (!value) {
        return "-";
    }

    return new Date(value).toLocaleString();
}

washCustomer.addEventListener(
    "change",
    event => {
        populateVehicleSelector(
            event.target.value
        );
    }
);

customerForm.addEventListener(
    "submit",
    addCustomer
);

vehicleForm.addEventListener(
    "submit",
    addVehicle
);

startWashForm.addEventListener(
    "submit",
    startWash
);

refreshButton.addEventListener(
    "click",
    loadDashboard
);
setInterval(updateWashTimers, 1000);
loadDashboard();