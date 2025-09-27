<script lang="ts" setup>
import { ref } from "vue";
import { getUser, saveUser } from "@/services/api";
import type { Bet } from "@/interfaces/Bet";
import { useRoute, useRouter } from "vue-router";
import BetResultDialog from "@/components/BetResultDialog.vue";
import SaveConfirmationDialog from "@/components/SaveConfirmationDialog.vue";

const route = useRoute();
const router = useRouter();

const betType = ref<"color" | "parity" | "number" | null>(null);
const betColor = ref<"red" | "black" | null>(null);
const betParity = ref<"even" | "odd" | null>(null);
const betNumber = ref<number | null>(null);
const amount = ref<number>(0);
const loading = ref(false);
const confirmDialog = ref(false);
const resultDialog = ref(false);
const resultMessage = ref("");
const balance = ref(0);

const playerName = route.params.user as string;
const betAmountRules = [
  (v: number) => !!v || "El monto es obligatorio",
  (v: number) => v > 0 || "El monto debe ser mayor que 0",
];

const loadGame = async () => {
  try {
    const user = await getUser(playerName);
    balance.value = user.balance;
  } catch (error) {
    console.log(error);
  }
};
loadGame();

const spinRoulette = () => {
  const number = Math.floor(Math.random() * 37);
  const color = Math.random() < 0.5 ? "red" : "black";
  return { number, color };
};

const calculateLocalPayout = (bet: Bet) => {
  let won = false;
  let payout = 0;

  if (bet.betType === "color") {
    if (bet.betColor === bet.result.color) {
      won = true;
      payout = bet.amount * 2; // gana el doble
    }
  } else if (bet.betType === "parity") {
    const isEven = bet.result.number % 2 === 0;
    if (
      bet.result.number !== 0 &&
      ((betParity.value === "even" && isEven) ||
        (betParity.value === "odd" && !isEven))
    ) {
      won = true;
      payout = bet.amount * 2;
    }
  } else if (bet.betType === "number") {
    if (bet.number === bet.result.number) {
      won = true;
      payout = bet.amount * 36;
    }
  }

  return { won, payout };
};

const placeBet = async () => {
  if (!betType.value || amount.value <= 0) return;

  if (amount.value > balance.value) {
    resultMessage.value = "No tienes suficiente balance para esta apuesta.";
    resultDialog.value = true;
    return;
  }

  loading.value = true;
  try {
    const result = spinRoulette();

    const bet: Bet = {
      betType: betType.value,
      betColor: betColor.value ?? "",
      number: betNumber.value ?? 0,
      amount: amount.value,
      result: result,
    };

    const payoutResult = calculateLocalPayout(bet);

    if (payoutResult.won) {
      balance.value += payoutResult.payout;
    } else {
      balance.value = Math.max(0, balance.value - amount.value);
    }

    resultMessage.value = payoutResult.won
      ? `¡Ganaste! Premio: $${payoutResult.payout}`
      : "Lo siento, perdiste 😢";

    resultDialog.value = true;
  } catch (error) {
    console.error(error);
  } finally {
    loading.value = false;
  }
};

const saveGame = async () => {
  try {
    await saveUser({
      name: playerName,
      balance: balance.value,
    });
    router.push("/");
  } catch (error) {
    console.error("Error al guardar:", error);
  }
};

const discardGame = () => {
  router.push("/");
};
</script>

<template>
  <v-container class="game-container" fluid>
    <v-btn
      icon="mdi-arrow-left"
      class="back-btn"
      color="grey"
      @click="confirmDialog = true"
    />
    <h1 class="text-center mb-6">
      {{ `Jugando: ${playerName}` }}
    </h1>
    <div class="grid-wrapper">
      <!-- Botones de números (1–36 en 3 columnas) -->
      <v-btn
        v-for="num in 36"
        :key="num"
        :color="num % 2 === 0 ? 'red' : 'black'"
        class="number-btn"
        :class="{ selected: betType === 'number' && betNumber === num }"
        @click="
          () => {
            betType = 'number';
            betNumber = num;
          }
        "
      >
        {{ num }}
      </v-btn>

      <!-- Opciones de apuestas -->
      <v-btn
        class="option-btn parity"
        :class="{ selected: betType === 'parity' && betParity === 'even' }"
        color="grey"
        @click="
          () => {
            betType = 'parity';
            betParity = 'even';
          }
        "
      >
        Par
      </v-btn>

      <v-btn
        class="option-btn rojo"
        :class="{ selected: betType === 'color' && betColor === 'red' }"
        color="red"
        @click="
          () => {
            betType = 'color';
            betColor = 'red';
          }
        "
      >
        Rojo
      </v-btn>

      <v-btn
        class="option-btn negro"
        :class="{ selected: betType === 'color' && betColor === 'black' }"
        color="black"
        @click="
          () => {
            betType = 'color';
            betColor = 'black';
          }
        "
      >
        Negro
      </v-btn>

      <v-btn
        class="option-btn impar"
        :class="{ selected: betType === 'parity' && betParity === 'odd' }"
        color="grey"
        @click="
          () => {
            betType = 'parity';
            betParity = 'odd';
          }
        "
      >
        Impar
      </v-btn>
    </div>

    <!-- Barra inferior -->
    <v-footer class="footer-bar">
      <v-container>
        <v-row align="center" justify="space-between">
          <v-col cols="4" class="text-right">
            <strong class="text-white">{{ `Balance: ${balance}` }}</strong>
          </v-col>

          <!-- Monto a apostar -->
          <v-col cols="4">
            <v-text-field
              v-model.number="amount"
              label="Monto a apostar"
              type="number"
              variant="outlined"
              density="compact"
              class="text-white px-5"
              :rules="betAmountRules"
              hide-details
            />
          </v-col>

          <!-- Botón apostar -->
          <v-col cols="4">
            <v-btn color="warning" :loading="loading" @click="placeBet">
              Apostar
            </v-btn>
          </v-col>
        </v-row>
      </v-container>
    </v-footer>
  </v-container>

  <BetResultDialog
    v-model="resultDialog"
    :bet-type="betType"
    :bet-color="betColor"
    :bet-parity="betParity"
    :bet-number="betNumber"
    :result-message="resultMessage"
    @exit="confirmDialog = true"
  />
  <SaveConfirmationDialog
    v-model="confirmDialog"
    @save="saveGame"
    @discard="discardGame"
  />
</template>

<style scoped>
.game-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  background: url("../assets/bg.jpg") no-repeat center center;
  background-size: cover;
  padding: 20px;
}

.back-btn {
  position: absolute;
  top: 10px;
  left: 10px;
  background-color: rgba(0, 0, 0, 0.4);
}

.grid-wrapper {
  display: grid;
  /* grid-template-columns: repeat(12, 5vw); */
  grid-template-columns: repeat(12, 1fr);
  gap: 3px;
  justify-content: center;
}

.number-btn {
  height: 60px;
  font-weight: bold;
}

.option-btn {
  height: 60px;
  font-weight: bold;
}

.selected {
  filter: brightness(0.2);
}
.parity {
  grid-column: 2 / 5;
}

.rojo {
  grid-column: 5 / 7;
}

.negro {
  grid-column: 7 / 9;
}

.impar {
  grid-column: 9 / 12;
}

.footer-bar {
  background-color: #121212;
  border-top: 2px solid #444;
  position: fixed;
  bottom: 0;
  width: 100%;
}
</style>
