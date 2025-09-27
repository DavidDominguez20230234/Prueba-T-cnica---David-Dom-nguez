<script lang="ts" setup>
import LoadGameDialog from "@/components/LoadGameDialog.vue";
import { User } from "@/interfaces/User";
import { getSavedGames, saveUser } from "@/services/api";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const userName = ref("");
const initialAmount = ref(0);
const savedGames = ref<User[]>([]);
const loadingList = ref(false);
const loadingStartGame = ref(false);
const showList = ref(false);
const showLoadGameDialog = ref(false);
const selectedGame = ref("");
const gameExists = ref(false);
const formRef = ref();

const nameRules = [(v: string) => !!v || "El nombre es obligatorio"];

const amountRules = [
  (v: number) => !!v || "El monto es obligatorio",
  (v: number) => v > 0 || "El monto debe ser mayor que 0",
];

const loadSavedGames = async () => {
  loadingList.value = true;

  try {
    savedGames.value = await getSavedGames();
    showList.value = true;
  } catch {
  } finally {
    loadingList.value = false;
  }
};

const validateForm = () => {
  if (!userName.value) return;
  const isSaved = savedGames.value.find((x) => x.name == userName.value);
  if (isSaved) {
    selectedGame.value = userName.value;
    gameExists.value = true;
    showLoadGameDialog.value = true;
    return;
  } else {
    onStartGame();
  }
};

const onSuccess = () => {
  showLoadGameDialog.value = false;
  gameExists.value = false;
};

const onStartGame = async () => {
  loadingStartGame.value = true;
  try {
    await saveUser({ name: userName.value, balance: initialAmount.value });

    router.push({ name: "game", params: { user: userName.value } });
  } catch (error) {
    console.error(error);
  } finally {
    loadingStartGame.value = false;
    onSuccess();
  }
};

const openLoadGameDialog = (name: string) => {
  selectedGame.value = name;
  showLoadGameDialog.value = true;
};

onMounted(() => {
  userName.value = "";
  loadSavedGames();
});

const onValidate = async () => {
  const { valid } = await formRef.value.validate();
  if (valid) {
    validateForm();
  }
};
</script>

<template>
  <v-container class="start-container" fluid>
    <v-row align="center" justify="center" class="center-row">
      <v-col cols="12" v-if="loadingList" class="text-center">
        <v-progress-circular color="warning" indeterminate size="60" />
      </v-col>

      <v-col v-else cols="12" lg="4" md="6" sm="6">
        <h1 class="text-center mb-6">
          {{
            savedGames.length && showList
              ? "Partidas Guardadas"
              : "Iniciar Juego"
          }}
        </h1>

        <div v-if="savedGames.length && showList">
          <v-card class="mt-6 saved-card" variant="outlined">
            <div class="saved-list">
              <v-card
                v-for="user in savedGames"
                :key="user.name"
                class="saved-item mb-2"
                @click="openLoadGameDialog(user.name)"
              >
                <v-card-text class="d-flex justify-space-between">
                  <span>{{ user.name }}</span>
                  <strong>${{ user.balance }}</strong>
                </v-card-text>
              </v-card>
            </div>
          </v-card>

          <v-btn
            class="mt-4"
            color="info"
            variant="elevated"
            size="55"
            @click="showList = false"
            block
          >
            Nueva partida
          </v-btn>
        </div>

        <v-form v-else ref="formRef">
          <v-text-field
            v-model="userName"
            label="Nombre de usuario"
            variant="solo"
            :rules="nameRules"
          />

          <v-text-field
            v-model.number="initialAmount"
            label="Monto inicial"
            type="number"
            variant="solo"
            :rules="amountRules"
          />

          <v-btn
            color="warning"
            :loading="loadingStartGame"
            variant="elevated"
            size="55"
            block
            @click="onValidate"
          >
            Empezar
          </v-btn>
          <v-btn
            v-if="savedGames.length"
            class="mt-3"
            color="info"
            variant="elevated"
            size="55"
            block
            @click="showList = true"
          >
            Cargar Partida
          </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-container>

  <LoadGameDialog
    v-model="showLoadGameDialog"
    :game-name="selectedGame"
    :game-exists="gameExists"
    @cancel="onSuccess"
    @overwrite="onStartGame"
  />
</template>
<style scoped>
.start-container {
  min-height: 100vh;
  background: url("../assets/bg.jpg") no-repeat center center;
  background-size: cover;

  display: flex;
  align-items: center;
  justify-content: center;
}

.saved-card {
  max-height: 300px;
  overflow-y: auto;
}

.saved-list {
  padding: 8px;
}

.saved-item {
  background: #2a2a2a;
  color: white;
  cursor: pointer;
  transition: background 0.2s;
}
.saved-item:hover {
  background: #3a3a3a;
}
</style>
