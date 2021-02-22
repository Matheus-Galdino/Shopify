<template>
  <div class="shopping-list-container">
    <header>
      <img src="../assets/source.svg" alt="bottle" />
      <h3>Didn't find what you need?</h3>
      <button @click="$emit('add-item')">Add item</button>
    </header>

    <main>
      <template v-if="ShoppingList.groups">
        <div class="list-name-container">
          <h2 v-show="!isEditing">{{ ShoppingList.name }}</h2>
          <input
            type="text"
            v-model="updateName"
            v-show="isEditing"
            placeholder="Enter new list name"
          />

          <span
            class="material-icons"
            @click="isEditing = true"
            v-show="!isEditing"
          >
            edit
          </span>
          <button @click="update" v-show="isEditing">
            <span class="material-icons"> check </span>
          </button>
          <span
            class="material-icons cancel-button"
            @click="isEditing = false"
            v-show="isEditing"
          >
            clear
          </span>
        </div>

        <div class="list-items">
          <section
            class="category-group"
            v-for="group in ShoppingList.groups"
            :key="group.name"
          >
            <small>{{ group.name }}</small>
            <div class="items">
              <ShoppingListItem
                v-for="item in group.items"
                :key="item.id"
                :baseItem="item"
                :listId="ShoppingList.id"
                :initialQuantity="item.quantity"
                :isInStats="displayStatsInfo"
                @has-removed-item="refreshList"
                @refresh-chart="$emit('refresh-chart')"
              />
            </div>
          </section>
        </div>
      </template>

      <div class="hero-container" v-else>
        <p>No items</p>
      </div>
    </main>

    <footer class="add-footer" v-show="!displayStatsInfo">
      <form
        class="input-group"
        :class="{ disabled: !canNext }"
        @submit.prevent="save"
      >
        <input
          type="text"
          placeholder="Enter a name"
          v-model="newListName"
          v-show="!createNextStep"
          required
        />
        <button @click="createNextStep = true" v-show="!createNextStep">
          Next
        </button>

        <input
          type="date"
          v-model="newListDate"
          v-show="createNextStep"
          placeholder="Enter new list date"
          required
        />
        <span
          v-show="createNextStep"
          class="material-icons return-button"
          @click="createNextStep = false"
        >
          clear
        </span>
        <button :class="{ disabled: !canSave }" v-show="createNextStep">
          Save
        </button>
      </form>
    </footer>

    <footer class="complete-footer" v-show="displayStatsInfo && ShoppingList.groups != null">      
      <button @click="popupVisible = true">Cancel</button>
      <button class="complete" @click="updateStatus('completed')">
        Complete
      </button>
    </footer>

    <Popup
      @cancel="popupVisible = false"
      @confirm="updateStatus('cancelled')"
      v-show="popupVisible"
      content="Are you sure that you want to cancel this list?"
    />
  </div>
</template>

<script>
import API from "../API";

import Popup from "../components/Popup";
import ShoppingListItem from "../components/ShoppingListItem";

export default {
  name: "ShoppingList",
  components: {
    ShoppingListItem,
    Popup,
  },
  props: {
    ShoppingList: {
      type: Object,
    },
    displayStatsInfo: {
      type: Boolean,
    },
  },
  data() {
    return {
      updateName: "",
      newListName: "",
      newListDate: "",
      isEditing: false,
      popupVisible: false,
      createNextStep: false,
    };
  },
  methods: {
    async update() {
      await API.updateShoppingList(this.ShoppingList.id, this.updateName);
      this.updateName = "";
      this.isEditing = false;
      this.refreshList();
      this.$emit("show-notification", ["List name updated", "success"]);
    },
    async save() {
      if (!this.canSave) return;
      
      const result = await API.createShoppingList(
        this.newListName,
        this.newListDate
      );
      
      if(result instanceof Error){
        this.$emit("show-notification", [result.message, "error"]);        
        return;
      }

      this.newListName = "";
      this.newListDate = "";
      this.createNextStep = false;
      this.refreshList();
      this.$emit("show-notification", ["List created", "success"]);
    },
    async updateStatus(status) {
      this.popupVisible = false;
      await API.changeListStatus(this.ShoppingList.id, status);
      this.refreshList();
      this.$emit("show-notification", [`List ${status}`, "success"]);
    },
    refreshList() {
      this.$emit("refresh-list");
    },
  },
  computed: {
    canNext() {
      return this.newListName.length >= 3;
    },
    canSave() {
      return this.newListDate;
    },
  },
};
</script>

<style scoped>
/*#region container */
.shopping-list-container {
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  background: #fff0de;
}

.shopping-list-container header {
  display: flex;
  flex-wrap: wrap;
  position: relative;

  padding: 1.8rem 2.8rem;
  padding-left: 12rem;
  margin: 4rem 4rem 0;

  border-radius: 24px;
  background-color: #80485b;
}

.shopping-list-container header img {
  position: absolute;
  left: 10%;
  top: -20px;
}

.shopping-list-container header h3 {
  color: white;
  font-size: 1.6rem;
  font-weight: bold;
}

.shopping-list-container header button {
  color: #34333a;
  font-weight: bold;
  font-size: 1.4rem;

  cursor: pointer;
  padding: 1rem 3rem;
  margin-top: 1.3rem;

  border: none;
  border-radius: 12px;
  background: #ffffff;
  box-shadow: 0px 2px 12px rgba(0, 0, 0, 0.04);
}
/*#endregion */

/*#region main */
.shopping-list-container main {
  flex-grow: 1;
  padding: 0 4rem;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
}

.shopping-list-container main .list-name-container {
  display: flex;
  align-items: center;
  justify-content: space-between;

  margin-top: 4.4rem;
  margin-bottom: 4rem;
}

.list-name-container h2 {
  color: #34333a;
  font-size: 2.4rem;
}

.list-name-container .material-icons {
  cursor: pointer;
}

.list-name-container input {
  border-radius: 12px;
  padding: 1rem;
  border: 2px solid #bdbdbd;
  width: 80%;
  margin-right: 1rem;
}

.list-name-container button {
  display: flex;
  column-gap: 0.5rem;
  align-items: center;

  color: #fff;
  font-size: 1.3rem;
  font-weight: bold;
  background: #f9a109;

  border: none;
  border-radius: 12px;

  cursor: pointer;
  padding: 0.7rem;
}

.list-name-container .cancel-button {
  color: #e63946;
  margin-left: 0.5rem;
}

.category-group{
  margin-bottom: 4rem;
}

.category-group:last-child{
  margin-bottom: 2rem;
}

.category-group small {
  font-weight: 500;
  color: #828282;
  font-size: 1.4rem;

  display: block;
  margin-bottom: 2.5rem;
}

.category-group ul {
  list-style: none;
}

main .hero-container {
  min-height: 100%;

  display: flex;
  align-items: center;
  justify-content: center;
  flex-grow: 1;

  background-repeat: no-repeat;
  background-image: url("../assets/shopping.svg");
  background-position: bottom;
}

.hero-container p {
  font-size: 2rem;
  color: #34333a;
  font-weight: bold;
}

/*#endregion */

footer {
  width: 100%;
  padding: 3.5rem;

  display: flex;
  align-items: center;
  background: #ffffff;
}

/*#region add footer */

.add-footer .input-group {
  width: 100%;
  display: flex;
  border-radius: 12px;
  border: 2px solid #f9a109;
}

.add-footer .input-group * {
  border: none;
}

.add-footer .input-group input {
  flex-grow: 1;
  padding: 2rem 0;
  padding-left: 1.5rem;
  border-radius: inherit;
}

.add-footer .input-group button {
  color: #fff;
  font-size: 16px;
  font-weight: bold;
  border-radius: 10px;
  background: #f9a109;

  cursor: pointer;
  padding: 2rem 2.5rem;
}

.add-footer .input-group.disabled {
  border-color: #c1c1c4;
}

.add-footer .input-group.disabled button {
  background: #c1c1c4;
  cursor: not-allowed;
}

.add-footer .return-button {
  display: flex;
  cursor: pointer;
  margin: 0 0.5rem;
  color: #e63946;
  align-items: center;
}

/*#endregion*/

/*#region complete footer */
.complete-footer {
  justify-content: center;
  column-gap: 2rem;
}

.complete-footer button {
  padding: 2rem;
  font-size: 16px;
  font-weight: bold;

  border: none;
  cursor: pointer;
  background: transparent;
}

.complete-footer .complete {
  color: #fff;
  border-radius: 12px;
  background: #56ccf2;
}

/*#endregion */
</style>
