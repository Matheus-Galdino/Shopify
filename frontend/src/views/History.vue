<template>
  <div id="history-tab">
    <template v-if="!selectedList">
      <h2>Shopping History</h2>

      <div class="lists">
        <section class="list-group" v-for="group in AllLists" :key="group.date">
          <h3>{{ formatGroupDate(group.date) }}</h3>

          <div class="list" v-for="list in group.shoppingLists" :key="list.id">
            <div class="name-container">
              <input
                type="radio"
                name="activeList"
                id="activeList"
                :checked="list.isActive"
                v-show="list.status === 'Progress'"
                @change="changeActive(list.id)"
              />
              <p>
                {{ list.name }}
              </p>
            </div>

            <div class="list-details">
              <span class="material-icons"> event_note </span>
              <small>{{ new Date(list.date).toDateString() }}</small>
              <span class="status" :class="true && list.status">
                {{ list.status }}
              </span>
              <span
                class="material-icons delete-button"
                @click="deleteList(list.id)"
              >
                delete_outline
              </span>
              <span
                class="material-icons select-button"
                @click="selectedList = list"
              >
                keyboard_arrow_right
              </span>
            </div>
          </div>
        </section>
      </div>
    </template>

    <ShoppingListDetails
      v-else
      :shoppingListId="selectedList.id"
      @back="selectedList = undefined"
    />
  
  </div>
</template>

<script>
import API from "@/API";
import ShoppingListDetails from "../components/ShoppingListDetails";

export default {
  name: "History",
  components: {
    ShoppingListDetails,
  },
  props: {
    AllLists: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      selectedList: undefined,      
    };
  },
  methods: {
    async deleteList(listId) {
      await API.deleteList(listId);

      this.$emit("show-notification", ["List has been deleted", "success"]);
      this.$emit("refresh-lists");
      this.$emit('refresh-chart');
    },
    async changeActive(listId) {
      await API.setActiveList(listId);

      this.$emit("show-notification", ["Active list changed", "success"]);
      this.$emit("refresh-lists");
    },    
    formatGroupDate(stringDate) {
      const Months = [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December",
      ];

      const date = new Date(stringDate);

      return `${Months[date.getMonth()]} ${date.getFullYear()}`;
    },
  },  
};
</script>

<style scoped>
#history-tab {
  overflow-y: auto;
  padding-right: 6.5rem;
  padding-bottom: 4rem;
}

#history-tab h2 {
  color: #34333a;
  font-size: 2.6rem;
  margin-bottom: 4rem;
}

#history-tab .lists {
  display: flex;
  flex-direction: column;
  row-gap: 5.4rem;
}

.list-group h3 {
  font-weight: 600;
  color: #000000;
  font-size: 1.4rem;
  margin-bottom: 1.7rem;
}

.list-group .list {
  display: flex;
  align-items: center;
  justify-content: space-between;

  padding: 2rem;
  border-radius: 12px;
  background: #ffffff;
  box-shadow: 0px 2px 12px rgba(0, 0, 0, 0.05);
}

.list-group .list + .list {
  margin-top: 3rem;
}

.list .name-container {
  display: flex;
  column-gap: 1.5rem;
  align-items: center;
}

.name-container p {
  font-weight: 500;
  font-size: 1.7rem;
}

.list .list-details {
  display: flex;
  align-items: center;
  column-gap: 1.3rem;
}

.list-details .material-icons,
.list-details small {
  color: #c1c1c4;
}

.list-details small {
  font-weight: 500;
  font-size: 1.2rem;
}

.list-details .status {
  display: block;
  font-size: 1.2rem;
  border: 1px solid;
  border-radius: 8px;
  margin-left: 1.5rem;
  padding: 0.5rem 0.7rem;
}

.status.Progress {
  color: #56ccf2;
  border-color: #56ccf2;
}

.status.Completed {
  color: #56eb56;
  border-color: #56eb56;
}

.status.Cancelled {
  color: #eb5757;
  border-color: #eb5757;
}

.list-details .delete-button {
  cursor: pointer;
  color: #eb5757;
}

.list-details .select-button {
  cursor: pointer;
  color: #f9a109;
}
</style>
